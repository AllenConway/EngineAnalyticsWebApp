using EngineAnalyticsWebApp.Shared.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace EngineAnalyticsWebApp.Shared.Tests.Extensions
{
    public class ServiceCollectionExtensionsTests
    {
        [Fact]
        public void AddValidationForSharedTypes_ReturnsSameServiceCollection()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            var result = services.AddValidationForSharedTypes();

            // Assert
            Assert.Same(services, result);
        }

        [Fact]
        public void AddValidationForSharedTypes_RegistersValidationServices()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.AddValidationForSharedTypes();

            // Assert
            // Validation registration adds services to the collection
            Assert.NotEmpty(services);
        }
    }
}
