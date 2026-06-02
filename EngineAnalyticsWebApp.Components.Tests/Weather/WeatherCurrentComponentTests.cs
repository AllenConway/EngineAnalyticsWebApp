using Bunit;
using EngineAnalyticsWebApp.Components.Weather;
using EngineAnalyticsWebApp.Components.Weather.Services;
using EngineAnalyticsWebApp.Shared.Models.Weather;
using EngineAnalyticsWebApp.Shared.Services.Data;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace EngineAnalyticsWebApp.Components.Tests.Weather
{
    public class WeatherCurrentComponentTests : BunitContext
    {
        private readonly Mock<IWeatherService> _weatherServiceMock;
        private readonly Mock<IWeatherDataService> _weatherDataServiceMock;
        private readonly Subject<string> _zipCodeStream = new();

        public WeatherCurrentComponentTests()
        {
            _weatherServiceMock = new Mock<IWeatherService>();
            _weatherDataServiceMock = new Mock<IWeatherDataService>();

            _weatherServiceMock.Setup(s => s.GetCurrentZipCodeStream()).Returns(_zipCodeStream.AsObservable());

            Services.AddSingleton(_weatherServiceMock.Object);
            Services.AddSingleton(_weatherDataServiceMock.Object);
        }

        [Fact]
        public void WeatherCurrent_OnInitialized_SubscribesToZipCodeStream()
        {
            // Arrange
            // Act
            var cut = Render<WeatherCurrent>();

            // Assert
            _weatherServiceMock.Verify(s => s.GetCurrentZipCodeStream(), Times.Once);
        }

        [Fact]
        public void WeatherCurrent_WhenZipCodeStreamed_LoadsCurrentWeather()
        {
            // Arrange
            var current = new Current
            {
                Name = "Las Vegas",
                Weather = new[] { new Overview { Main = "Clear" } },
                Main = new Main { Temp = 75, TempMax = 80, TempMin = 60, Humidity = 20 },
                Wind = new Wind { Speed = 5 }
            };
            _weatherDataServiceMock.Setup(s => s.GetCurrentWeather(It.IsAny<string>())).ReturnsAsync(current);
            var cut = Render<WeatherCurrent>();

            // Act
            cut.InvokeAsync(() => _zipCodeStream.OnNext("89109"));

            // Assert
            cut.WaitForAssertion(() => Assert.Contains("Las Vegas", cut.Markup));
        }

        [Fact]
        public void WeatherCurrent_WithNoWeatherData_RendersEmpty()
        {
            // Arrange
            // Act
            var cut = Render<WeatherCurrent>();

            // Assert
            // No zip streamed, so no weather card rendered
            Assert.DoesNotContain("Weather Location", cut.Markup);
        }

        [Fact]
        public void WeatherCurrent_WhenDisposed_DoesNotThrow()
        {
            // Arrange
            var cut = Render<WeatherCurrent>();

            // Act
            var exception = Record.Exception(() => cut.Instance.Dispose());

            // Assert
            Assert.Null(exception);
        }
    }
}
