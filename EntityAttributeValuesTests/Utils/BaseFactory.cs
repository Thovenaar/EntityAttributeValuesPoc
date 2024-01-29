using EntityAttributeValues;
using EntityAttributeValues.Storage;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace EntityAttributeValuesTests.Utils;

public abstract class BaseFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    public HttpClient HttpClient { get; private set; } = default;

    private readonly DatabaseContainer _dbContext = new();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(ConfigureServices);

        builder.UseEnvironment("IntegrationTests");
    }

    private void ConfigureServices(IServiceCollection services)
    {
        services.RemoveAll<DbContextOptions<AppDbContext>>();
        services.RemoveAll(typeof(AppDbContext));

        services.AddDbContext<AppDbContext>(o => o.UseSqlServer(_dbContext.GetConnectionString()));
    }

    public async Task ResetDatabaseAsync()
    {
        await _dbContext.ResetDatabaseAsync();

        await SeedData();
    }

    public async Task InitializeAsync()
    {
        await _dbContext.InitializeAsync();

        var dbContext = CreateDbContext();
        await dbContext.Database.EnsureCreatedAsync();

        await SeedData();

        await _dbContext.InitializeRespawner();

        HttpClient = CreateClient();
    }

    public new async Task DisposeAsync() => await _dbContext.Dispose();
    public abstract Task SeedData();
    internal AppDbContext CreateDbContext() => new(_dbContext.GetDbContextOptions<AppDbContext>());
}