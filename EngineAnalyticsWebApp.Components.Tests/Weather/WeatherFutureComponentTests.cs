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
    public class WeatherFutureComponentTests : BunitContext
    {
        private readonly Mock<IWeatherService> _weatherServiceMock;
        private readonly Mock<IWeatherDataService> _weatherDataServiceMock;
        private readonly Subject<string> _zipCodeStream = new();

        public WeatherFutureComponentTests()
        {
            _weatherServiceMock = new Mock<IWeatherService>();
            _weatherDataServiceMock = new Mock<IWeatherDataService>();

            _weatherServiceMock.Setup(s => s.GetCurrentZipCodeStream()).Returns(_zipCodeStream.AsObservable());

            Services.AddSingleton(_weatherServiceMock.Object);
            Services.AddSingleton(_weatherDataServiceMock.Object);
        }

        [Fact]
        public void WeatherFuture_OnInitialized_SubscribesToZipCodeStream()
        {
            // Arrange
            // Act
            var cut = Render<WeatherFuture>();

            // Assert
            _weatherServiceMock.Verify(s => s.GetCurrentZipCodeStream(), Times.Once);
        }

        [Fact]
        public void WeatherFuture_WhenZipCodeStreamed_LoadsFutureWeather()
        {
            // Arrange
            var future = new Future
            {
                City = new ForecastCity { Name = "Las Vegas" },
                List = new[]
                {
                    new ForecastItem
                    {
                        DtTxt = "2023-01-01 12:00:00",
                        Main = new Main { TempMax = 80, TempMin = 60 },
                        Weather = new[] { new Overview { Main = "Clear" } }
                    }
                }
            };
            _weatherDataServiceMock.Setup(s => s.GetFutureWeather(It.IsAny<string>())).ReturnsAsync(future);
            var cut = Render<WeatherFuture>();

            // Act
            cut.InvokeAsync(() => _zipCodeStream.OnNext("89109"));

            // Assert
            cut.WaitForAssertion(() => Assert.Contains("Las Vegas", cut.Markup));
        }

        [Fact]
        public void WeatherFuture_WithNoData_RendersEmpty()
        {
            // Arrange
            // Act
            var cut = Render<WeatherFuture>();

            // Assert
            Assert.DoesNotContain("5-Day Forecast", cut.Markup);
        }

        [Fact]
        public void WeatherFuture_WhenDisposed_DoesNotThrow()
        {
            // Arrange
            var cut = Render<WeatherFuture>();

            // Act
            var exception = Record.Exception(() => cut.Instance.Dispose());

            // Assert
            Assert.Null(exception);
        }
    }
}
