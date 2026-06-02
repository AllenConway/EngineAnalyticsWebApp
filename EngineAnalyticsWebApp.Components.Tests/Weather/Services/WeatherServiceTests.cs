using Blazored.LocalStorage;
using EngineAnalyticsWebApp.Components.Weather.Services;
using Moq;
using System.Reactive.Linq;

namespace EngineAnalyticsWebApp.Components.Tests.Weather.Services
{
    public class WeatherServiceTests
    {
        private readonly Mock<ILocalStorageService> _localStorageMock;
        private readonly WeatherService _sut;

        public WeatherServiceTests()
        {
            _localStorageMock = new Mock<ILocalStorageService>();
            _sut = new WeatherService(_localStorageMock.Object);
        }

        [Fact]
        public void GetCurrentWeatherStream_ReturnsObservable()
        {
            // Arrange
            // Act
            var stream = _sut.GetCurrentWeatherStream();

            // Assert
            Assert.NotNull(stream);
        }

        [Fact]
        public void GetCurrentZipCodeStream_ReturnsObservable()
        {
            // Arrange
            // Act
            var stream = _sut.GetCurrentZipCodeStream();

            // Assert
            Assert.NotNull(stream);
        }

        [Fact]
        public async Task SetWeatherZipCode_WithNewZipCode_StoresAndStreamsValue()
        {
            // Arrange
            _localStorageMock
                .Setup(s => s.GetItemAsync<string>(It.IsAny<string>(), default))
                .ReturnsAsync((string?)null!);

            string? streamedZip = null;
            using var subscription = _sut.GetCurrentZipCodeStream().Subscribe(z => streamedZip = z);

            // Act
            await _sut.SetWeatherZipCode("90210");

            // Assert
            Assert.Equal("90210", streamedZip);
            _localStorageMock.Verify(s => s.SetItemAsync(It.IsAny<string>(), "90210", default), Times.Once);
        }

        [Fact]
        public async Task SetWeatherZipCode_WithNullAndNoStoredValue_UsesDefaultZipCode()
        {
            // Arrange
            _localStorageMock
                .Setup(s => s.GetItemAsync<string>(It.IsAny<string>(), default))
                .ReturnsAsync((string?)null!);

            string? streamedZip = null;
            using var subscription = _sut.GetCurrentZipCodeStream().Subscribe(z => streamedZip = z);

            // Act
            await _sut.SetWeatherZipCode(null);

            // Assert
            // Default zip code is 89109
            Assert.Equal("89109", streamedZip);
        }

        [Fact]
        public async Task SetWeatherZipCode_WithSameZipCodeAsStored_DoesNotStreamAgain()
        {
            // Arrange
            _localStorageMock
                .Setup(s => s.GetItemAsync<string>(It.IsAny<string>(), default))
                .ReturnsAsync("89109");

            var streamCount = 0;
            using var subscription = _sut.GetCurrentZipCodeStream().Subscribe(_ => streamCount++);

            // Act
            await _sut.SetWeatherZipCode("89109");

            // Assert
            Assert.Equal(0, streamCount);
            _localStorageMock.Verify(s => s.SetItemAsync(It.IsAny<string>(), It.IsAny<string>(), default), Times.Never);
        }

        [Fact]
        public async Task SetWeatherZipCode_WithNullButStoredValueExists_UsesStoredValue()
        {
            // Arrange
            _localStorageMock
                .Setup(s => s.GetItemAsync<string>(It.IsAny<string>(), default))
                .ReturnsAsync("12345");

            string? streamedZip = null;
            using var subscription = _sut.GetCurrentZipCodeStream().Subscribe(z => streamedZip = z);

            // Act
            await _sut.SetWeatherZipCode(null);

            // Assert
            // Null with a stored value: 'zipCode != lastSetWeatherZipCode' is true (null != "12345"),
            // then zipCode is set to stored value before streaming
            Assert.Equal("12345", streamedZip);
        }
    }
}
