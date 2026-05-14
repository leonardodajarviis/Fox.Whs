using System.Security.Claims;
using System.Text.Json;
using Fox.Whs.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Fox.Whs.Data;

public class AuditSaveChangesInterceptor : SaveChangesInterceptor
{
    private static readonly HashSet<Type> AuditedTypes =
    [
        typeof(BlowingProcess),
        typeof(BlowingProcessLine),
        typeof(CuttingProcess),
        typeof(CuttingProcessLine),
        typeof(GrainMixingBlowingProcess),
        typeof(GrainMixingBlowingProcessLine),
        typeof(GrainMixingProcess),
        typeof(GrainMixingProcessLine),
        typeof(PrintingProcess),
        typeof(PrintingProcessLine),
        typeof(RewindingProcess),
        typeof(RewindingProcessLine),
        typeof(SlittingProcess),
        typeof(SlittingProcessLine)
    ];

    private static readonly HashSet<string> IgnoredPropertyNames =
    [
        "RowVersion"
    ];

    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);
    private readonly IHttpContextAccessor _httpContextAccessor;

    // Entity Added cần defer ghi audit cho tới khi PK do DB sinh ra (sau SavedChanges).
    private readonly List<EntityEntry> _deferredAddedEntries = new();

    // Flag để tránh tái nhập khi BackfillAddedAudits gọi SaveChanges đệ quy.
    private bool _isFlushingDeferred;

    public AuditSaveChangesInterceptor(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        WriteAuditLogs(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        WriteAuditLogs(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public override int SavedChanges(SaveChangesCompletedEventData eventData, int result)
    {
        FlushDeferredAddedAudits(eventData.Context);
        return base.SavedChanges(eventData, result);
    }

    public override async ValueTask<int> SavedChangesAsync(
        SaveChangesCompletedEventData eventData,
        int result,
        CancellationToken cancellationToken = default)
    {
        await FlushDeferredAddedAuditsAsync(eventData.Context, cancellationToken);
        return await base.SavedChangesAsync(eventData, result, cancellationToken);
    }

    public override void SaveChangesFailed(DbContextErrorEventData eventData)
    {
        _deferredAddedEntries.Clear();
        base.SaveChangesFailed(eventData);
    }

    public override Task SaveChangesFailedAsync(DbContextErrorEventData eventData, CancellationToken cancellationToken = default)
    {
        _deferredAddedEntries.Clear();
        return base.SaveChangesFailedAsync(eventData, cancellationToken);
    }

    private void WriteAuditLogs(DbContext? context)
    {
        if (context is null || _isFlushingDeferred)
        {
            return;
        }

        _deferredAddedEntries.Clear();

        var auditSet = context.Set<AuditLog>();

        foreach (var entry in context.ChangeTracker.Entries().Where(ShouldAudit).ToList())
        {
            // Added với key auto-generated: defer để lấy được PK thật sau khi save.
            if (entry.State == EntityState.Added && HasDbGeneratedKey(entry))
            {
                _deferredAddedEntries.Add(entry);
                continue;
            }

            var audit = BuildAuditLog(entry);
            if (audit is not null)
            {
                auditSet.Add(audit);
            }
        }
    }

    private void FlushDeferredAddedAudits(DbContext? context)
    {
        if (context is null || _isFlushingDeferred || _deferredAddedEntries.Count == 0)
        {
            return;
        }

        try
        {
            _isFlushingDeferred = true;
            AppendDeferredAuditLogs(context);
            context.SaveChanges();
        }
        finally
        {
            _isFlushingDeferred = false;
            _deferredAddedEntries.Clear();
        }
    }

    private async Task FlushDeferredAddedAuditsAsync(DbContext? context, CancellationToken cancellationToken)
    {
        if (context is null || _isFlushingDeferred || _deferredAddedEntries.Count == 0)
        {
            return;
        }

        try
        {
            _isFlushingDeferred = true;
            AppendDeferredAuditLogs(context);
            await context.SaveChangesAsync(cancellationToken);
        }
        finally
        {
            _isFlushingDeferred = false;
            _deferredAddedEntries.Clear();
        }
    }

    private void AppendDeferredAuditLogs(DbContext context)
    {
        var auditSet = context.Set<AuditLog>();
        foreach (var entry in _deferredAddedEntries)
        {
            var audit = BuildAuditLog(entry, forceAddedAction: true);
            if (audit is not null)
            {
                auditSet.Add(audit);
            }
        }
    }

    private static bool HasDbGeneratedKey(EntityEntry entry)
    {
        var pk = entry.Metadata.FindPrimaryKey();
        if (pk is null)
        {
            return false;
        }

        return pk.Properties.Any(p => p.ValueGenerated != Microsoft.EntityFrameworkCore.Metadata.ValueGenerated.Never);
    }

    private bool ShouldAudit(EntityEntry entry)
    {
        if (entry.Entity is AuditLog)
        {
            return false;
        }

        if (!AuditedTypes.Contains(entry.Entity.GetType()))
        {
            return false;
        }

        return entry.State is EntityState.Added or EntityState.Modified or EntityState.Deleted;
    }

    private AuditLog? BuildAuditLog(EntityEntry entry, bool forceAddedAction = false)
    {
        // Khi build deferred (sau SaveChanges), entry.State đã là Unchanged → cần ép action "Added".
        var effectiveState = forceAddedAction ? EntityState.Added : entry.State;

        var oldValues = new Dictionary<string, object?>();
        var newValues = new Dictionary<string, object?>();

        if (effectiveState == EntityState.Modified)
        {
            foreach (var property in entry.Properties.Where(p => p.IsModified))
            {
                if (IgnoredPropertyNames.Contains(property.Metadata.Name))
                {
                    continue;
                }

                var original = property.OriginalValue;
                var current = property.CurrentValue;
                if (ValuesEqual(original, current))
                {
                    continue;
                }

                oldValues[property.Metadata.Name] = original;
                newValues[property.Metadata.Name] = current;
            }

            if (oldValues.Count == 0 && newValues.Count == 0)
            {
                return null;
            }
        }
        else if (effectiveState == EntityState.Added)
        {
            foreach (var property in entry.Properties)
            {
                if (IgnoredPropertyNames.Contains(property.Metadata.Name))
                {
                    continue;
                }

                newValues[property.Metadata.Name] = property.CurrentValue;
            }
        }
        else if (effectiveState == EntityState.Deleted)
        {
            foreach (var property in entry.Properties)
            {
                if (IgnoredPropertyNames.Contains(property.Metadata.Name))
                {
                    continue;
                }

                oldValues[property.Metadata.Name] = property.OriginalValue;
            }
        }

        var keyValues = ReadKeyValues(entry, useOriginal: effectiveState == EntityState.Deleted);

        var user = _httpContextAccessor.HttpContext?.User;
        var userIdClaim = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var parsedUserId = short.TryParse(userIdClaim, out var userId) ? userId : (short?)null;
        var username = user?.FindFirst(ClaimTypes.Name)?.Value;
        var ipAddress = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();

        return new AuditLog
        {
            EntityName = entry.Metadata.ClrType.Name,
            Action = effectiveState.ToString(),
            KeyValuesJson = keyValues.Count == 0 ? null : JsonSerializer.Serialize(keyValues, JsonOptions),
            OldValuesJson = oldValues.Count == 0 ? null : JsonSerializer.Serialize(oldValues, JsonOptions),
            NewValuesJson = newValues.Count == 0 ? null : JsonSerializer.Serialize(newValues, JsonOptions),
            UserId = parsedUserId,
            Username = username,
            IpAddress = ipAddress,
            CreatedAt = DateTime.UtcNow
        };
    }

    private static Dictionary<string, object?> ReadKeyValues(EntityEntry entry, bool useOriginal)
    {
        var keyValues = new Dictionary<string, object?>();
        var primaryKey = entry.Metadata.FindPrimaryKey();
        if (primaryKey is null)
        {
            return keyValues;
        }

        foreach (var keyProperty in primaryKey.Properties)
        {
            var prop = entry.Property(keyProperty.Name);
            keyValues[keyProperty.Name] = useOriginal ? prop.OriginalValue : prop.CurrentValue;
        }

        return keyValues;
    }

    private static bool ValuesEqual(object? a, object? b)
    {
        if (ReferenceEquals(a, b)) return true;
        if (a is null || b is null) return false;
        if (a is byte[] arrA && b is byte[] arrB) return arrA.AsSpan().SequenceEqual(arrB);
        return Equals(a, b);
    }
}
