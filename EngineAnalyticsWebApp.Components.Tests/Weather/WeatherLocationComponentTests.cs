using Bunit;
using EngineAnalyticsWebApp.Components.Weather;
using EngineAnalyticsWebApp.Components.Weather.Services;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace EngineAnalyticsWebApp.Components.Tests.Weather
{
    public class WeatherLocationComponentTests : BunitContext
    {
        private readonly Mock<IWeatherService> _weatherServiceMock;

        public WeatherLocationComponentTests()
        {
            // Arrange: WeatherLocation imports a JS module on first render; loose mode tolerates this
            JSInterop.Mode = JSRuntimeMode.Loose;

            _weatherServiceMock = new Mock<IWeatherService>();
            _weatherServiceMock.Setup(s => s.SetWeatherZipCode(It.IsAny<string?>())).Returns(Task.CompletedTask);

            Services.AddSingleton(_weatherServiceMock.Object);
        }

        [Fact]
        public void WeatherLocation_RendersSearchbarAndButton()
        {
            // Arrange
            // Act
            var cut = Render<WeatherLocation>();

            // Assert
            Assert.NotNull(cut.Find("ion-searchbar"));
            Assert.NotNull(cut.Find("ion-button"));
        }

        [Fact]
        public void WeatherLocation_OnInitialized_SetsWeatherZipCode()
        {
            // Arrange
            // Act
            var cut = Render<WeatherLocation>();

            // Assert
            _weatherServiceMock.Verify(s => s.SetWeatherZipCode(It.IsAny<string?>()), Times.Once);
        }

        [Fact]
        public async Task WeatherLocation_WhenDisposed_DoesNotThrow()
        {
            // Arrange
            var cut = Render<WeatherLocation>();

            // Act
            var exception = await Record.ExceptionAsync(async () => await cut.Instance.DisposeAsync());

            // Assert
            Assert.Null(exception);
        }
    }
}
