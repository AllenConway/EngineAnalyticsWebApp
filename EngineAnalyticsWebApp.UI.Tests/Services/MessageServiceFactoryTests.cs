using EngineAnalyticsWebApp.Shared.Services;
using EngineAnalyticsWebApp.UI.Services;

namespace EngineAnalyticsWebApp.UI.Tests.Services
{
    public class MessageServiceFactoryTests
    {
        [Fact]
        public void Create_ReturnsMessageServiceInstance()
        {
            // Arrange
            var factory = new MessageServiceFactory();

            // Act
            var service = factory.Create();

            // Assert
            Assert.NotNull(service);
            Assert.IsAssignableFrom<IMessageService>(service);
        }

        [Fact]
        public void Create_CalledMultipleTimes_ReturnsNewInstanceEachTime()
        {
            // Arrange
            var factory = new MessageServiceFactory();

            // Act
            var service1 = factory.Create();
            var service2 = factory.Create();

            // Assert
            Assert.NotSame(service1, service2);
        }

        [Fact]
        public void Create_ReturnedService_CanLogMessages()
        {
            // Arrange
            var factory = new MessageServiceFactory();
            var service = factory.Create();

            // Act
            var result = service.MessageLogger("test message");

            // Assert
            Assert.Equal("You logged: 'test message'", result);
        }
    }
}
