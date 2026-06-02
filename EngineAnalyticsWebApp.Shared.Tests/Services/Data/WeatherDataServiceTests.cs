using EngineAnalyticsWebApp.Shared.Models.Weather;
using EngineAnalyticsWebApp.Shared.Services.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using System.Net;
using System.Net.Http.Json;

namespace EngineAnalyticsWebApp.Shared.Tests.Services.Data
{
    public class WeatherDataServiceTests
    {
        private readonly Mock<ILogger<WeatherDataService>> _loggerMock;
        private readonly IConfiguration _configuration;

        public WeatherDataServiceTests()
        {
            _loggerMock = new Mock<ILogger<WeatherDataService>>();
            var inMemorySettings = new Dictionary<string, string?>
            {
                { "OpenWeatherMap:ApiKey", "test-api-key" }
            };
            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
        }

        [Fact]
        public void Constructor_WithMissingApiKey_ThrowsInvalidOperationException()
        {
            // Arrange
            var emptyConfig = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>())
                .Build();
            var handler = new MockHttpMessageHandler(new HttpResponseMessage(HttpStatusCode.OK));
            var httpClient = new HttpClient(handler) { BaseAddress = new Uri("https://api.example.com/") };

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() =>
                new WeatherDataService(httpClient, emptyConfig, _loggerMock.Object));
        }

        [Fact]
        public async Task GetCurrentWeather_WhenApiReturnsData_ReturnsCurrentWeather()
        {
            // Arrange
            var current = new Current { Name = "Las Vegas", Id = 123 };
            var handler = new MockHttpMessageHandler(
                new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = JsonContent.Create(current)
                });
            var httpClient = new HttpClient(handler) { BaseAddress = new Uri("https://api.example.com/") };
            var sut = new WeatherDataService(httpClient, _configuration, _loggerMock.Object);

            // Act
            var result = await sut.GetCurrentWeather("89109");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Las Vegas", result.Name);
        }

        [Fact]
        public async Task GetCurrentWeather_WhenApiThrowsHttpException_ReturnsEmptyCurrent()
        {
            // Arrange
            var handler = new MockHttpMessageHandler(
                new HttpResponseMessage(HttpStatusCode.InternalServerError));
            var httpClient = new HttpClient(handler) { BaseAddress = new Uri("https://api.example.com/") };
            var sut = new WeatherDataService(httpClient, _configuration, _loggerMock.Object);

            // Act
            var result = await sut.GetCurrentWeather("00000");

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetFutureWeather_WhenApiReturnsData_ReturnsFutureWeather()
        {
            // Arrange
            var future = new Future { Cod = "200", Cnt = 5 };
            var handler = new MockHttpMessageHandler(
                new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = JsonContent.Create(future)
                });
            var httpClient = new HttpClient(handler) { BaseAddress = new Uri("https://api.example.com/") };
            var sut = new WeatherDataService(httpClient, _configuration, _loggerMock.Object);

            // Act
            var result = await sut.GetFutureWeather("89109");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("200", result.Cod);
        }

        [Fact]
        public async Task GetFutureWeather_WhenApiThrowsHttpException_ReturnsEmptyFuture()
        {
            // Arrange
            var handler = new MockHttpMessageHandler(
                new HttpResponseMessage(HttpStatusCode.InternalServerError));
            var httpClient = new HttpClient(handler) { BaseAddress = new Uri("https://api.example.com/") };
            var sut = new WeatherDataService(httpClient, _configuration, _loggerMock.Object);

            // Act
            var result = await sut.GetFutureWeather("00000");

            // Assert
            Assert.NotNull(result);
        }

        private class MockHttpMessageHandler(HttpResponseMessage response) : HttpMessageHandler
        {
            protected override Task<HttpResponseMessage> SendAsync(
                HttpRequestMessage request, CancellationToken cancellationToken)
            {
                return Task.FromResult(response);
            }
        }
    }
}
