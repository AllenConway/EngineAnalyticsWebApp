using EngineAnalyticsWebApp.Shared.Models.Weather;

namespace EngineAnalyticsWebApp.Shared.Tests.Models.Weather
{
    public class GeolocationTests
    {
        [Fact]
        public void Geolocation_DefaultValues_AreNull()
        {
            // Arrange
            // Act
            var geo = new Geolocation();

            // Assert
            Assert.Null(geo.Latitude);
            Assert.Null(geo.Longitude);
        }

        [Fact]
        public void Geolocation_CanSetValues()
        {
            // Arrange
            // Act
            var geo = new Geolocation { Latitude = 36.1699, Longitude = -115.1398 };

            // Assert
            Assert.Equal(36.1699, geo.Latitude);
            Assert.Equal(-115.1398, geo.Longitude);
        }
    }
}
