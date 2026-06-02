using EngineAnalyticsWebApp.Shared.Models.Weather;
using System.Text.Json;

namespace EngineAnalyticsWebApp.Shared.Tests.Models.Weather
{
    public class FutureTests
    {
        [Fact]
        public void Future_DefaultValues_AreCorrect()
        {
            // Arrange
            // Act
            var future = new Future();

            // Assert
            Assert.Null(future.Cod);
            Assert.Equal(0, future.Cnt);
            Assert.Null(future.List);
            Assert.Null(future.City);
        }

        [Fact]
        public void Future_CanDeserializeFromJson()
        {
            // Arrange
            var json = """{"cod":"200","cnt":5,"list":[],"city":null}""";

            // Act
            var result = JsonSerializer.Deserialize<Future>(json);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("200", result.Cod);
            Assert.Equal(5, result.Cnt);
            Assert.NotNull(result.List);
            Assert.Empty(result.List);
        }

        [Fact]
        public void Future_WithForecastItems_DeserializesCorrectly()
        {
            // Arrange
            var json = """{"cod":"200","cnt":1,"list":[{"dt":1234567890,"dt_txt":"2023-01-01 12:00:00"}]}""";

            // Act
            var result = JsonSerializer.Deserialize<Future>(json);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.List);
            Assert.Single(result.List);
            Assert.Equal("2023-01-01 12:00:00", result.List[0].DtTxt);
        }
    }
}
