using EntityAttributeValues.Models;
using EntityAttributeValuesTests.Utils;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace EntityAttributeValuesTests.Tests
{
    [Collection(SharedTestCollections.CommonTestsCollection)]
    public class WhenReceivingProductAttributes : IAsyncLifetime
    {
        private readonly CommonTestsFactory _factory;

        public WhenReceivingProductAttributes(CommonTestsFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public void Test1()
        {
            // Arrange
            var dbContext = _factory.CreateDbContext();
            dbContext.Products.Add(new Product
            {
                Id = Guid.NewGuid(),
                Attributes =
                {
                    new ProductAttribute
                    {
                        Id = Guid.NewGuid()
                    }
                }
            });

            dbContext.SaveChanges();

            var client = _factory.HttpClient;

            // Act
            var dbContext2 = _factory.CreateDbContext();
            var products = dbContext2.Products.Include(p => p.Attributes).ToList();

            // Assert
            products.Should().HaveCount(1);
        }

        public Task InitializeAsync() => Task.CompletedTask;
        public Task DisposeAsync() => _factory.ResetDatabaseAsync();
    }
}