using EngineAnalyticsWebApp.TestLazy.Messaging.Services;

namespace EngineAnalyticsWebApp.Shared.Tests.Services
{
    public class MessageServiceTests
    {
        private readonly MessageService _sut = new();

        [Fact]
        public void MessageLogger_WithValidMessage_ReturnsFormattedString()
        {
            // Arrange
            var message = "Hello World";

            // Act
            var result = _sut.MessageLogger(message);

            // Assert
            Assert.Equal("You logged: 'Hello World'", result);
        }

        [Fact]
        public void MessageLogger_WithEmptyMessage_ReturnsFormattedEmptyString()
        {
            // Arrange
            var message = "";

            // Act
            var result = _sut.MessageLogger(message);

            // Assert
            Assert.Equal("You logged: ''", result);
        }

        [Fact]
        public void MessageLogger_WithSpecialCharacters_ReturnsFormattedString()
        {
            // Arrange
            var message = "Test <>&\"'";

            // Act
            var result = _sut.MessageLogger(message);

            // Assert
            Assert.Contains(message, result);
        }
    }
}
