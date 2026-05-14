using Fox.Whs.Models;
using Fox.Whs.SapModels;
using Microsoft.EntityFrameworkCore;

namespace Fox.Whs.Tests;

/// <summary>
/// DbContext tối giản chỉ chứa các entity cần thiết để test AuditSaveChangesInterceptor.
/// Bỏ qua navigation properties để tránh kéo theo User/Employee/ProductionOrder.
/// </summary>
public class TestDbContext : DbContext
{
    public TestDbContext(DbContextOptions<TestDbContext> options) : base(options)
    {
    }

    public DbSet<AuditLog> AuditLogs { get; set; } = null!;
    public DbSet<BlowingProcess> BlowingProcesses { get; set; } = null!;
    public DbSet<BlowingProcessLine> BlowingProcessLines { get; set; } = null!;
    public DbSet<IdempotencyKey> IdempotencyKeys { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Loại bỏ hoàn toàn các entity ngoài phạm vi test (kéo theo User → UserPermission → ...).
        modelBuilder.Ignore<User>();
        modelBuilder.Ignore<UserPermission>();
        modelBuilder.Ignore<Employee>();

        modelBuilder.Entity<BlowingProcess>(b =>
        {
            b.Ignore(x => x.ShiftLeader);
            b.Ignore(x => x.Creator);
            b.Ignore(x => x.Modifier);

            // SQLite không hỗ trợ rowversion auto-gen → cấu hình thành cột BLOB thường, giá trị set thủ công trong test.
            b.Property(x => x.RowVersion)
                .IsConcurrencyToken(false)
                .ValueGeneratedNever();

            b.HasMany(x => x.Lines)
                .WithOne(l => l.BlowingProcess)
                .HasForeignKey(l => l.BlowingProcessId);
        });

        modelBuilder.Entity<BlowingProcessLine>(b =>
        {
            b.Ignore(x => x.Worker);
        });
    }
}
