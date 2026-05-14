using Fox.Whs.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Fox.Whs.Tests;

public abstract class AuditInterceptorTestBase : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly DbContextOptions<TestDbContext> _options;
    protected readonly IHttpContextAccessor HttpContextAccessor;
    protected readonly AuditSaveChangesInterceptor Interceptor;

    protected AuditInterceptorTestBase()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();

        HttpContextAccessor = new HttpContextAccessor();
        Interceptor = new AuditSaveChangesInterceptor(HttpContextAccessor);

        _options = new DbContextOptionsBuilder<TestDbContext>()
            .UseSqlite(_connection)
            .AddInterceptors(Interceptor)
            .Options;

        using var ctx = new TestDbContext(_options);
        ctx.Database.EnsureCreated();
    }

    protected TestDbContext CreateContext() => new(_options);

    public void Dispose()
    {
        _connection.Dispose();
        GC.SuppressFinalize(this);
    }
}
