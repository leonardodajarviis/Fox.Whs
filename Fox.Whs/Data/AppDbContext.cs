using Fox.Whs.Models;
using Fox.Whs.SapModels;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Fox.Whs.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<UserPermission> UserPermissions { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<BusinessPartner> BusinessPartners { get; set; }
    public DbSet<Blower> Blowers { get; set; }
    public DbSet<ProductionOrder> ProductionOrders { get; set; }
    public DbSet<Item> Items { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<BlowingProcess> BlowingProcesses { get; set; }
    public DbSet<BlowingProcessLine> BlowingProcessLines { get; set; }
    public DbSet<ProductionShift> ProductionShifts { get; set; }

    public DbSet<PrintingProcess> PrintingProcesses { get; set; }
    public DbSet<PrintingProcessLine> PrintingProcessLines { get; set; }
    public DbSet<Printer> Printers { get; set; }
    public DbSet<RewindingMachine> RewindingMachines { get; set; }

    public DbSet<CuttingProcess> CuttingProcesses { get; set; }
    public DbSet<CuttingProcessLine> CuttingProcessLines { get; set; }
    public DbSet<CuttingMachine> CuttingMachines { get; set; }

    public DbSet<SlittingProcess> SlittingProcesses { get; set; }
    public DbSet<SlittingMachine> SlittingMachines { get; set; }
    public DbSet<SlittingProcessLine> SlittingProcessLines { get; set; }
    public DbSet<RewindingProcess> RewindingProcesses { get; set; }
    public DbSet<RewindingProcessLine> RewindingProcessLines { get; set; }

    public DbSet<GrainMixingProcess> GrainMixingProcesses { get; set; }
    public DbSet<GrainMixingProcessLine> GrainMixingProcessLines { get; set; }

    public DbSet<GrainMixingBlowingProcess> GrainMixingBlowingProcesses { get; set; }
    public DbSet<GrainMixingBlowingProcessLine> GrainMixingBlowingProcessLines { get; set; }

    public DbSet<MixingMachine> MixingMachines { get; set; }
    public DbSet<ProductionOrderGrainMixing> ProductionOrderGrainMixings { get; set; }

    public DbSet<UserSession> UserSessions { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var clrType = entityType.ClrType;
            if (clrType.GetCustomAttributes(typeof(ReadOnlyEntityAttribute), inherit: false).Length != 0)
            {
                modelBuilder.Entity(clrType).ToTable(t => t.ExcludeFromMigrations());
            }
        }

        modelBuilder.Entity<UserPermission>(up =>
        {
            up.HasKey(perm => new { perm.Code, perm.LineId });
        });

        // Áp dụng các configuration
        // modelBuilder.ApplyConfigurationsFromAssembly(typeof(SapDbContext).Assembly);
    }

    public async Task ExecuteInTransactionAsync(Func<Task> action)
    {
        await using var transaction = await Database.BeginTransactionAsync();

        try
        {
            await action();
            await transaction.CommitAsync();
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<TResult> ExecuteInTransactionAsync<TResult>(Func<Task<TResult>> action)
    {
        await using var transaction = await Database.BeginTransactionAsync();

        try
        {
            var result = await action();
            await transaction.CommitAsync();
            return result;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<int> UpdateStatusProductionOrderSapAsync(string fieldName, int[] sapProductionOrderIds)
    {
        if (sapProductionOrderIds == null || sapProductionOrderIds.Length == 0)
            return 0;

        // Chỉ cho phép update những cột hợp lệ thôi
        var allowedFields = new[] { "U_THOISTATUS", "U_INSTATUS", "U_CATSTATUS", "U_TUASTATUS", "U_CHIASTATUS" };
        if (!allowedFields.Contains(fieldName))
            throw new ArgumentException("Invalid field name");

        // Tạo danh sách parameters an toàn
        var parameters = sapProductionOrderIds
            .Select((id, i) => new SqlParameter($"@p{i}", id))
            .ToArray();

        var inClause = string.Join(", ", parameters.Select(p => p.ParameterName));

        var sql = $"UPDATE dbo.OWOR SET {fieldName} = 'Y' WHERE DocEntry IN ({inClause})";

        return await Database.ExecuteSqlRawAsync(sql, parameters);
    }
}
