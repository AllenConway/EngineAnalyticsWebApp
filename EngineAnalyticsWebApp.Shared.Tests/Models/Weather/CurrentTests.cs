using EngineAnalyticsWebApp.Shared.Models.Weather;
using System.Text.Json;

namespace EngineAnalyticsWebApp.Shared.Tests.Models.Weather
{
    public class CurrentTests
    {
        [Fact]
        public void Current_DefaultValues_AreCorrect()
        {
            // Arrange
            // Act
            var current = new Current();

            // Assert
            Assert.Null(current.Coord);
            Assert.Null(current.Weather);
            Assert.Null(current.Name);
            Assert.Null(current.Main);
            Assert.Null(current.Wind);
            Assert.Null(current.Clouds);
            Assert.Null(current.Twilight);
            Assert.Equal(0, current.Visibility);
            Assert.Equal(0, current.Dt);
            Assert.Equal(0, current.Timezone);
            Assert.Equal(0, current.Id);
            Assert.Equal(0, current.Cod);
        }

        [Fact]
        public void Current_CanDeserializeFromJson()
        {
            // Arrange
            var json = """{"name":"Las Vegas","id":5506956,"cod":200}""";

            // Act
            var result = JsonSerializer.Deserialize<Current>(json);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Las Vegas", result.Name);
            Assert.Equal(5506956, result.Id);
            Assert.Equal(200, result.Cod);
        }

        [Fact]
        public void Current_CanSerializeToJson()
        {
            // Arrange
            var current = new Current { Name = "Test City", Id = 123, Cod = 200 };

            // Act
            var json = JsonSerializer.Serialize(current);
            var deserialized = JsonSerializer.Deserialize<Current>(json);

            // Assert
            Assert.NotNull(deserialized);
            Assert.Equal("Test City", deserialized.Name);
        }
    }
}
