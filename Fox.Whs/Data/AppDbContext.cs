using BackEnd_LHC.SapModels;
using Fox.Whs.Models;
using Fox.Whs.SapModels;
using Microsoft.EntityFrameworkCore;

namespace Fox.Whs.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

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

        modelBuilder.Entity<UserGroupAssignment>(u =>
        {
            u.HasKey(uga => new { uga.UserId, uga.GroupId });
        });

        // Áp dụng các configuration
        // modelBuilder.ApplyConfigurationsFromAssembly(typeof(SapDbContext).Assembly);
    }
}
