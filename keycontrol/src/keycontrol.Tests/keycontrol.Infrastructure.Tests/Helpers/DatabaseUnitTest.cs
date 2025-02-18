using keycontrol.Infrastructure.Context;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace keycontrol.Tests.Helpers;

public abstract class DatabaseUnitTest : IDisposable
{
    private const string InMemoryConnectionString = "DataSource=:memory:";
    private readonly SqliteConnection _connection;
    protected readonly AppDbContextForTests _dbContext;
    private bool isDisposed;
    protected DatabaseUnitTest()
    {
        _connection = new SqliteConnection(InMemoryConnectionString);
        _connection.Open();

        var dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
        .UseSqlite("Filename=UnitTests.db")
        .Options;

        _dbContext = new AppDbContextForTests(dbContextOptions);
        _dbContext.Database.EnsureCreated();
    }
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);

    }
    protected virtual void Dispose(bool disposing)
    {
        if (isDisposed) return;

        if (disposing)
        {
            _dbContext.Database.CloseConnection();

        }
        isDisposed = true;
    }
}