using System.Data.Common;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Respawn;

namespace EntityAttributeValuesTests.Utils
{
    public class DatabaseContainer
    {
        private DbConnection _dbConnection = default!;
        private Respawner _dbRespawner = default!;

        private readonly MsSqlTestcontainer _dbContainer =
            new TestcontainersBuilder<MsSqlTestcontainer>()
                .WithEnvironment("ACCEPT_EULA", "Y")
                .WithDatabase(
                    new MsSqlTestcontainerConfiguration
                    {
                        Password = "UltraSecret92120!!"
                    })
                .Build();

        public async Task InitializeAsync()
        {
            await _dbContainer.StartAsync();

            _dbConnection = new SqlConnection(GetConnectionString());
        }

        public async Task InitializeRespawner()
        {
            await _dbConnection.OpenAsync();
            _dbRespawner = await Respawner.CreateAsync(_dbConnection,
                new RespawnerOptions { DbAdapter = DbAdapter.SqlServer });
        }

        public async Task ResetDatabaseAsync()
        {
            await _dbRespawner.ResetAsync(_dbConnection);
        }

        public async Task Dispose()
        {
            await _dbContainer.DisposeAsync();
        }

        public string GetConnectionString() =>
            $"{_dbContainer.ConnectionString};TrustServerCertificate=yes;";

        public DbContextOptions<T> GetDbContextOptions<T>() where T : DbContext
        {
            DbContextOptionsBuilder<T> optionsBuilder = new DbContextOptionsBuilder<T>();
            optionsBuilder.UseSqlServer(GetConnectionString());

            return optionsBuilder.Options;
        }
    }
}
