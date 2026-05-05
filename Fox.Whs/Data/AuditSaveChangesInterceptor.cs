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
        typeof(CuttingProcess),
        typeof(GrainMixingBlowingProcess),
        typeof(GrainMixingProcess),
        typeof(PrintingProcess),
        typeof(RewindingProcess),
        typeof(SlittingProcess)
    ];

    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);
    private readonly IHttpContextAccessor _httpContextAccessor;

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

    private void WriteAuditLogs(DbContext? context)
    {
        if (context is null)
        {
            return;
        }

        var auditEntries = context.ChangeTracker
            .Entries()
            .Where(ShouldAudit)
            .Select(BuildAuditLog)
            .Where(audit => audit is not null)
            .Cast<AuditLog>()
            .ToList();

        if (auditEntries.Count == 0)
        {
            return;
        }

        context.Set<AuditLog>().AddRange(auditEntries);
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

    private AuditLog? BuildAuditLog(EntityEntry entry)
    {
        var oldValues = new Dictionary<string, object?>();
        var newValues = new Dictionary<string, object?>();

        if (entry.State == EntityState.Modified)
        {
            foreach (var property in entry.Properties.Where(p => p.IsModified))
            {
                var original = property.OriginalValue;
                var current = property.CurrentValue;
                if (Equals(original, current))
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
        else if (entry.State == EntityState.Added)
        {
            foreach (var property in entry.Properties)
            {
                newValues[property.Metadata.Name] = property.CurrentValue;
            }
        }
        else if (entry.State == EntityState.Deleted)
        {
            foreach (var property in entry.Properties)
            {
                oldValues[property.Metadata.Name] = property.OriginalValue;
            }
        }

        var keyValues = new Dictionary<string, object?>();
        var primaryKey = entry.Metadata.FindPrimaryKey();
        if (primaryKey != null)
        {
            foreach (var keyProperty in primaryKey.Properties)
            {
                var value = entry.State == EntityState.Deleted
                    ? entry.Property(keyProperty.Name).OriginalValue
                    : entry.Property(keyProperty.Name).CurrentValue;

                keyValues[keyProperty.Name] = value;
            }
        }

        var user = _httpContextAccessor.HttpContext?.User;
        var userIdClaim = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var parsedUserId = short.TryParse(userIdClaim, out var userId) ? userId : (short?)null;
        var username = user?.FindFirst(ClaimTypes.Name)?.Value;
        var ipAddress = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();

        return new AuditLog
        {
            EntityName = entry.Metadata.ClrType.Name,
            Action = entry.State.ToString(),
            KeyValuesJson = keyValues.Count == 0 ? null : JsonSerializer.Serialize(keyValues, JsonOptions),
            OldValuesJson = oldValues.Count == 0 ? null : JsonSerializer.Serialize(oldValues, JsonOptions),
            NewValuesJson = newValues.Count == 0 ? null : JsonSerializer.Serialize(newValues, JsonOptions),
            UserId = parsedUserId,
            Username = username,
            IpAddress = ipAddress,
            CreatedAt = DateTime.UtcNow
        };
    }
}
