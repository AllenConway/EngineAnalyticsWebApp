using Bunit;
using EngineAnalyticsWebApp.Components.Weather.Services;
using EngineAnalyticsWebApp.Shared.Models.Weather;
using EngineAnalyticsWebApp.Shared.Services;
using EngineAnalyticsWebApp.Shared.Services.Data;
using EngineAnalyticsWebApp.Shared.Services.Factories;
using EngineAnalyticsWebApp.UI.Components.Pages.Weather;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace EngineAnalyticsWebApp.UI.Tests.Components.Pages.Weather
{
    public class TrackWeatherTests : BunitContext
    {
        private readonly Mock<IWeatherService> _weatherServiceMock;
        private readonly Mock<IWeatherDataService> _weatherDataServiceMock;
        private readonly Mock<IMessageServiceFactory> _messageServiceFactoryMock;
        private readonly Subject<string> _zipCodeStream = new();

        public TrackWeatherTests()
        {
            // Arrange: JS interop runs in loose mode because WeatherLocation imports a JS module
            JSInterop.Mode = JSRuntimeMode.Loose;

            _weatherServiceMock = new Mock<IWeatherService>();
            _weatherDataServiceMock = new Mock<IWeatherDataService>();
            _messageServiceFactoryMock = new Mock<IMessageServiceFactory>();

            _weatherServiceMock.Setup(s => s.GetCurrentZipCodeStream()).Returns(_zipCodeStream.AsObservable());
            _weatherServiceMock.Setup(s => s.SetWeatherZipCode(It.IsAny<string?>())).Returns(Task.CompletedTask);
            _weatherDataServiceMock.Setup(s => s.GetCurrentWeather(It.IsAny<string>())).ReturnsAsync(new Current());
            _weatherDataServiceMock.Setup(s => s.GetFutureWeather(It.IsAny<string>())).ReturnsAsync(new Future());

            var messageServiceMock = new Mock<IMessageService>();
            messageServiceMock.Setup(s => s.MessageLogger(It.IsAny<string>())).Returns("logged");
            _messageServiceFactoryMock.Setup(f => f.Create()).Returns(messageServiceMock.Object);

            Services.AddSingleton(_weatherServiceMock.Object);
            Services.AddSingleton(_weatherDataServiceMock.Object);
            Services.AddSingleton(_messageServiceFactoryMock.Object);
        }

        [Fact]
        public void TrackWeather_RendersTitle()
        {
            // Arrange
            // Act
            var cut = Render<TrackWeather>();

            // Assert
            Assert.Contains("Track Weather", cut.Markup);
        }

        [Fact]
        public void TrackWeather_OnInitialized_SetsWeatherZipCode()
        {
            // Arrange
            // Act
            var cut = Render<TrackWeather>();

            // Assert
            _weatherServiceMock.Verify(s => s.SetWeatherZipCode(It.IsAny<string?>()), Times.AtLeastOnce);
        }

        [Fact]
        public void TrackWeather_RendersMessageComponent()
        {
            // Arrange
            // Act
            var cut = Render<TrackWeather>();

            // Assert
            _messageServiceFactoryMock.Verify(f => f.Create(), Times.AtLeastOnce);
        }

        [Fact]
        public void TrackWeather_RendersWeatherChildComponents()
        {
            // Arrange
            // Act
            var cut = Render<TrackWeather>();

            // Assert
            // Both current and future weather components subscribe to the zip code stream
            _weatherServiceMock.Verify(s => s.GetCurrentZipCodeStream(), Times.AtLeast(2));
        }
    }
}
