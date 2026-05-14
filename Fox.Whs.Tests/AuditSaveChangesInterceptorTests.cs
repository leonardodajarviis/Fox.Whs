using System.Net;
using System.Security.Claims;
using System.Text.Json;
using Fox.Whs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Fox.Whs.Tests;

public class AuditSaveChangesInterceptorTests : AuditInterceptorTestBase
{
    private static BlowingProcess NewProcess(string shift = "Ca 1", decimal totalBlowing = 100m) => new()
    {
        ProductionDate = new DateTime(2026, 5, 14),
        ProductionShift = shift,
        TotalBlowingOutput = totalBlowing,
        TotalRewindingOutput = 0m,
        TotalReservedOutput = 0m,
        TotalBlowingLoss = 0m,
        Status = 0,
        CreatorId = 1,
        RowVersion = [0x01]
    };

    private static BlowingProcessLine NewLine(string itemCode = "ITEM-1") => new()
    {
        ItemCode = itemCode,
        QuantityRolls = 1m,
        QuantityKg = 10m,
        RewindOrSplitWeight = 0m,
        ReservedWeight = 0m,
        StopDurationMinutes = 0,
        WidthChange = 0m,
        InnerCoating = 0m,
        TrimmedEdge = 0m,
        ElectricalIssue = 0m,
        MaterialLossKg = 0m,
        HumanErrorKg = 0m,
        MachineErrorKg = 0m,
        OtherErrorKg = 0m,
        TotalLoss = 0m,
        ExcessPO = 0m,
        BlowingStageInventory = 0m,
        Status = 0
    };

    private static long? ReadKeyId(AuditLog log)
    {
        if (string.IsNullOrEmpty(log.KeyValuesJson)) return null;
        using var doc = JsonDocument.Parse(log.KeyValuesJson);
        return doc.RootElement.GetProperty("Id").GetInt64();
    }

    private void SetUser(short userId, string username = "tester")
    {
        var identity = new ClaimsIdentity(
        [
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            new Claim(ClaimTypes.Name, username)
        ], "Test");

        HttpContextAccessor.HttpContext = new DefaultHttpContext
        {
            User = new ClaimsPrincipal(identity)
        };
    }

    [Fact]
    public async Task Added_Parent_AuditLog_HasPositiveDbGeneratedId()
    {
        await using var ctx = CreateContext();
        var process = NewProcess();
        ctx.BlowingProcesses.Add(process);
        await ctx.SaveChangesAsync();

        var logs = await ctx.AuditLogs.Where(x => x.EntityName == nameof(BlowingProcess)).ToListAsync();
        Assert.Single(logs);
        Assert.Equal("Added", logs[0].Action);

        var keyId = ReadKeyId(logs[0]);
        Assert.NotNull(keyId);
        Assert.True(keyId > 0, $"Expected positive DB-generated Id, got {keyId}");
        Assert.Equal(process.Id, keyId);
    }

    [Fact]
    public async Task Added_ParentAndLines_EachHasPositiveId()
    {
        await using var ctx = CreateContext();
        var process = NewProcess();
        process.Lines.Add(NewLine("A"));
        process.Lines.Add(NewLine("B"));
        ctx.BlowingProcesses.Add(process);
        await ctx.SaveChangesAsync();

        var logs = await ctx.AuditLogs.OrderBy(x => x.Id).ToListAsync();
        Assert.Equal(3, logs.Count);

        Assert.All(logs, l =>
        {
            Assert.Equal("Added", l.Action);
            var keyId = ReadKeyId(l);
            Assert.NotNull(keyId);
            Assert.True(keyId > 0, $"{l.EntityName} has non-positive key {keyId}");
        });

        var parentLog = logs.Single(l => l.EntityName == nameof(BlowingProcess));
        Assert.Equal(process.Id, ReadKeyId(parentLog));

        var lineLogs = logs.Where(l => l.EntityName == nameof(BlowingProcessLine)).ToList();
        Assert.Equal(2, lineLogs.Count);
        var lineIds = lineLogs.Select(ReadKeyId).ToHashSet();
        Assert.Equal(process.Lines.Select(l => (long?)l.Id).ToHashSet(), lineIds);
    }

    [Fact]
    public async Task Modified_LogsOldAndNewValues_AndExcludesUnchangedAndIgnoredProps()
    {
        int processId;
        await using (var seed = CreateContext())
        {
            var p = NewProcess();
            seed.BlowingProcesses.Add(p);
            await seed.SaveChangesAsync();
            processId = p.Id;
        }

        // Xoá audit log của lần insert để chỉ kiểm tra log của lần update.
        await using (var clean = CreateContext())
        {
            clean.AuditLogs.RemoveRange(clean.AuditLogs);
            await clean.SaveChangesAsync();
        }

        await using (var ctx = CreateContext())
        {
            var p = await ctx.BlowingProcesses.SingleAsync(x => x.Id == processId);
            p.TotalBlowingOutput = 999m;
            p.Notes = "updated";
            await ctx.SaveChangesAsync();
        }

        await using var verify = CreateContext();
        var logs = await verify.AuditLogs.ToListAsync();
        Assert.Single(logs);
        var log = logs[0];
        Assert.Equal("Modified", log.Action);
        Assert.Equal(processId, ReadKeyId(log));

        using var oldDoc = JsonDocument.Parse(log.OldValuesJson!);
        using var newDoc = JsonDocument.Parse(log.NewValuesJson!);

        Assert.Equal(100m, oldDoc.RootElement.GetProperty(nameof(BlowingProcess.TotalBlowingOutput)).GetDecimal());
        Assert.Equal(999m, newDoc.RootElement.GetProperty(nameof(BlowingProcess.TotalBlowingOutput)).GetDecimal());
        Assert.Equal("updated", newDoc.RootElement.GetProperty(nameof(BlowingProcess.Notes)).GetString());

        // Các property không thay đổi không được log.
        Assert.False(newDoc.RootElement.TryGetProperty(nameof(BlowingProcess.ProductionShift), out _));

        // RowVersion bị ignore.
        Assert.False(newDoc.RootElement.TryGetProperty(nameof(BlowingProcess.RowVersion), out _));
        Assert.False(oldDoc.RootElement.TryGetProperty(nameof(BlowingProcess.RowVersion), out _));
    }

    [Fact]
    public async Task NoOpModified_DoesNotCreateAuditLog()
    {
        int processId;
        await using (var seed = CreateContext())
        {
            var p = NewProcess();
            seed.BlowingProcesses.Add(p);
            await seed.SaveChangesAsync();
            processId = p.Id;
        }

        await using (var clean = CreateContext())
        {
            clean.AuditLogs.RemoveRange(clean.AuditLogs);
            await clean.SaveChangesAsync();
        }

        await using (var ctx = CreateContext())
        {
            var p = await ctx.BlowingProcesses.SingleAsync(x => x.Id == processId);
            ctx.Entry(p).State = EntityState.Modified; // không đổi giá trị, chỉ force Modified
            await ctx.SaveChangesAsync();
        }

        await using var verify = CreateContext();
        var count = await verify.AuditLogs.CountAsync();
        Assert.Equal(0, count);
    }

    [Fact]
    public async Task Deleted_LogsOldValuesAndKey()
    {
        int processId;
        await using (var seed = CreateContext())
        {
            var p = NewProcess(shift: "Ca 2", totalBlowing: 500m);
            seed.BlowingProcesses.Add(p);
            await seed.SaveChangesAsync();
            processId = p.Id;
        }

        await using (var clean = CreateContext())
        {
            clean.AuditLogs.RemoveRange(clean.AuditLogs);
            await clean.SaveChangesAsync();
        }

        await using (var ctx = CreateContext())
        {
            var p = await ctx.BlowingProcesses.SingleAsync(x => x.Id == processId);
            ctx.BlowingProcesses.Remove(p);
            await ctx.SaveChangesAsync();
        }

        await using var verify = CreateContext();
        var log = await verify.AuditLogs.SingleAsync();
        Assert.Equal("Deleted", log.Action);
        Assert.Equal(processId, ReadKeyId(log));
        Assert.NotNull(log.OldValuesJson);
        Assert.Null(log.NewValuesJson);

        using var oldDoc = JsonDocument.Parse(log.OldValuesJson!);
        Assert.Equal("Ca 2", oldDoc.RootElement.GetProperty(nameof(BlowingProcess.ProductionShift)).GetString());
        Assert.Equal(500m, oldDoc.RootElement.GetProperty(nameof(BlowingProcess.TotalBlowingOutput)).GetDecimal());
    }

    [Fact]
    public async Task NonAuditedEntity_DoesNotCreateAuditLog()
    {
        await using var ctx = CreateContext();
        ctx.IdempotencyKeys.Add(new IdempotencyKey
        {
            Key = "k1",
            UserId = 1,
            Method = "POST",
            Path = "/x",
            RequestHash = "hash",
            ResponseStatus = 200,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddHours(1)
        });
        await ctx.SaveChangesAsync();

        var count = await ctx.AuditLogs.CountAsync();
        Assert.Equal(0, count);
    }

    [Fact]
    public async Task AuditLog_CapturesUserIdAndUsernameFromHttpContext()
    {
        SetUser(42, "alice");

        await using var ctx = CreateContext();
        ctx.BlowingProcesses.Add(NewProcess());
        await ctx.SaveChangesAsync();

        var log = await ctx.AuditLogs.SingleAsync();
        Assert.Equal((short)42, log.UserId);
        Assert.Equal("alice", log.Username);
    }

    [Fact]
    public void Added_SyncSaveChanges_AlsoBackfillsKey()
    {
        using var ctx = CreateContext();
        var process = NewProcess();
        ctx.BlowingProcesses.Add(process);
        ctx.SaveChanges(); // đường sync, không phải async

        var log = ctx.AuditLogs.Single(x => x.EntityName == nameof(BlowingProcess));
        Assert.Equal("Added", log.Action);
        var keyId = ReadKeyId(log);
        Assert.NotNull(keyId);
        Assert.True(keyId > 0);
        Assert.Equal(process.Id, keyId);
    }

    [Fact]
    public async Task Added_Log_HasNewValuesAndNullOldValues()
    {
        await using var ctx = CreateContext();
        ctx.BlowingProcesses.Add(NewProcess(shift: "Ca 3", totalBlowing: 77m));
        await ctx.SaveChangesAsync();

        var log = await ctx.AuditLogs.SingleAsync();
        Assert.Equal("Added", log.Action);
        Assert.Null(log.OldValuesJson);
        Assert.NotNull(log.NewValuesJson);

        using var newDoc = JsonDocument.Parse(log.NewValuesJson!);
        Assert.Equal("Ca 3", newDoc.RootElement.GetProperty(nameof(BlowingProcess.ProductionShift)).GetString());
        Assert.Equal(77m, newDoc.RootElement.GetProperty(nameof(BlowingProcess.TotalBlowingOutput)).GetDecimal());
        // RowVersion vẫn bị skip ở Added.
        Assert.False(newDoc.RootElement.TryGetProperty(nameof(BlowingProcess.RowVersion), out _));
    }

    [Fact]
    public async Task MixedStates_AddedModifiedDeleted_AllLogged()
    {
        int existingId;
        int deletingId;
        await using (var seed = CreateContext())
        {
            var keep = NewProcess(shift: "K", totalBlowing: 1m);
            var dele = NewProcess(shift: "D", totalBlowing: 2m);
            seed.BlowingProcesses.AddRange(keep, dele);
            await seed.SaveChangesAsync();
            existingId = keep.Id;
            deletingId = dele.Id;
        }

        await using (var clean = CreateContext())
        {
            clean.AuditLogs.RemoveRange(clean.AuditLogs);
            await clean.SaveChangesAsync();
        }

        await using (var ctx = CreateContext())
        {
            var keep = await ctx.BlowingProcesses.SingleAsync(x => x.Id == existingId);
            var dele = await ctx.BlowingProcesses.SingleAsync(x => x.Id == deletingId);
            keep.Notes = "edited";
            ctx.BlowingProcesses.Remove(dele);
            ctx.BlowingProcesses.Add(NewProcess(shift: "N", totalBlowing: 3m));
            await ctx.SaveChangesAsync();
        }

        await using var verify = CreateContext();
        var logs = await verify.AuditLogs.OrderBy(x => x.Id).ToListAsync();

        Assert.Equal(3, logs.Count);
        Assert.Contains(logs, l => l.Action == "Modified" && ReadKeyId(l) == existingId);
        Assert.Contains(logs, l => l.Action == "Deleted" && ReadKeyId(l) == deletingId);

        var addedLog = logs.Single(l => l.Action == "Added");
        var addedKeyId = ReadKeyId(addedLog);
        Assert.NotNull(addedKeyId);
        Assert.True(addedKeyId > 0);
        Assert.NotEqual(existingId, addedKeyId);
        Assert.NotEqual(deletingId, addedKeyId);
    }

    [Fact]
    public async Task NoHttpContext_LogsHaveNullUserAndIp()
    {
        HttpContextAccessor.HttpContext = null;

        await using var ctx = CreateContext();
        ctx.BlowingProcesses.Add(NewProcess());
        await ctx.SaveChangesAsync();

        var log = await ctx.AuditLogs.SingleAsync();
        Assert.Null(log.UserId);
        Assert.Null(log.Username);
        Assert.Null(log.IpAddress);
    }

    [Fact]
    public async Task IpAddress_CapturedFromConnection()
    {
        HttpContextAccessor.HttpContext = new DefaultHttpContext();
        HttpContextAccessor.HttpContext.Connection.RemoteIpAddress = IPAddress.Parse("10.20.30.40");

        await using var ctx = CreateContext();
        ctx.BlowingProcesses.Add(NewProcess());
        await ctx.SaveChangesAsync();

        var log = await ctx.AuditLogs.SingleAsync();
        Assert.Equal("10.20.30.40", log.IpAddress);
    }

    [Fact]
    public async Task InvalidUserIdClaim_LeavesUserIdNullButKeepsUsername()
    {
        var identity = new ClaimsIdentity(
        [
            new Claim(ClaimTypes.NameIdentifier, "not-a-number"),
            new Claim(ClaimTypes.Name, "bob")
        ], "Test");
        HttpContextAccessor.HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal(identity) };

        await using var ctx = CreateContext();
        ctx.BlowingProcesses.Add(NewProcess());
        await ctx.SaveChangesAsync();

        var log = await ctx.AuditLogs.SingleAsync();
        Assert.Null(log.UserId);
        Assert.Equal("bob", log.Username);
    }

    [Fact]
    public async Task RemoveLineFromParentCollection_LogsDeleted()
    {
        int processId;
        int lineToRemoveId;
        await using (var seed = CreateContext())
        {
            var p = NewProcess();
            p.Lines.Add(NewLine("KEEP"));
            p.Lines.Add(NewLine("DROP"));
            seed.BlowingProcesses.Add(p);
            await seed.SaveChangesAsync();
            processId = p.Id;
            lineToRemoveId = p.Lines.Single(l => l.ItemCode == "DROP").Id;
        }

        await using (var clean = CreateContext())
        {
            clean.AuditLogs.RemoveRange(clean.AuditLogs);
            await clean.SaveChangesAsync();
        }

        await using (var ctx = CreateContext())
        {
            var p = await ctx.BlowingProcesses.Include(x => x.Lines).SingleAsync(x => x.Id == processId);
            var line = p.Lines.Single(l => l.Id == lineToRemoveId);
            p.Lines.Remove(line);
            ctx.BlowingProcessLines.Remove(line);
            await ctx.SaveChangesAsync();
        }

        await using var verify = CreateContext();
        var logs = await verify.AuditLogs.ToListAsync();
        Assert.Single(logs);
        Assert.Equal(nameof(BlowingProcessLine), logs[0].EntityName);
        Assert.Equal("Deleted", logs[0].Action);
        Assert.Equal(lineToRemoveId, ReadKeyId(logs[0]));
    }

    [Fact]
    public async Task LineOnlyChange_StillProducesAuditLog()
    {
        int processId;
        int lineId;
        await using (var seed = CreateContext())
        {
            var p = NewProcess();
            p.Lines.Add(NewLine("X"));
            seed.BlowingProcesses.Add(p);
            await seed.SaveChangesAsync();
            processId = p.Id;
            lineId = p.Lines[0].Id;
        }

        await using (var clean = CreateContext())
        {
            clean.AuditLogs.RemoveRange(clean.AuditLogs);
            await clean.SaveChangesAsync();
        }

        await using (var ctx = CreateContext())
        {
            var line = await ctx.BlowingProcessLines.SingleAsync(x => x.Id == lineId);
            line.QuantityKg = 9999m;
            await ctx.SaveChangesAsync();
        }

        await using var verify = CreateContext();
        var logs = await verify.AuditLogs.ToListAsync();
        Assert.Single(logs);
        Assert.Equal(nameof(BlowingProcessLine), logs[0].EntityName);
        Assert.Equal("Modified", logs[0].Action);
        Assert.Equal(lineId, ReadKeyId(logs[0]));
    }
}
