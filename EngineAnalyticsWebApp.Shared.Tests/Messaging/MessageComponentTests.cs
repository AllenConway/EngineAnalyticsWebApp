using Bunit;
using EngineAnalyticsWebApp.Shared.Services;
using EngineAnalyticsWebApp.Shared.Services.Factories;
using EngineAnalyticsWebApp.TestLazy.Messaging;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace EngineAnalyticsWebApp.Shared.Tests.Messaging
{
    public class MessageComponentTests : BunitContext
    {
        private readonly Mock<IMessageServiceFactory> _messageServiceFactoryMock;
        private readonly Mock<IMessageService> _messageServiceMock;

        public MessageComponentTests()
        {
            _messageServiceMock = new Mock<IMessageService>();
            _messageServiceFactoryMock = new Mock<IMessageServiceFactory>();
            _messageServiceFactoryMock.Setup(f => f.Create()).Returns(_messageServiceMock.Object);

            Services.AddSingleton(_messageServiceFactoryMock.Object);
        }

        [Fact]
        public void MessageComponent_WithMessage_RendersFormattedMessage()
        {
            // Arrange
            _messageServiceMock.Setup(s => s.MessageLogger("Hello"))
                .Returns("You logged: 'Hello'");

            // Act
            var cut = Render<MessageComponent>(parameters => parameters
                .Add(p => p.Message, "Hello"));

            // Assert
            Assert.Contains("You logged: 'Hello'", cut.Markup);
        }

        [Fact]
        public void MessageComponent_WithMessage_InvokesMessageServiceFactory()
        {
            // Arrange
            _messageServiceMock.Setup(s => s.MessageLogger(It.IsAny<string>())).Returns("logged");

            // Act
            var cut = Render<MessageComponent>(parameters => parameters
                .Add(p => p.Message, "Hello"));

            // Assert
            _messageServiceFactoryMock.Verify(f => f.Create(), Times.Once);
            _messageServiceMock.Verify(s => s.MessageLogger("Hello"), Times.Once);
        }

        [Fact]
        public void MessageComponent_WithEmptyMessage_DoesNotInvokeLogger()
        {
            // Arrange
            // Act
            var cut = Render<MessageComponent>(parameters => parameters
                .Add(p => p.Message, string.Empty));

            // Assert
            _messageServiceMock.Verify(s => s.MessageLogger(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void MessageComponent_WithNullMessage_DoesNotInvokeLogger()
        {
            // Arrange
            // Act
            var cut = Render<MessageComponent>(parameters => parameters
                .Add(p => p.Message, (string?)null));

            // Assert
            _messageServiceMock.Verify(s => s.MessageLogger(It.IsAny<string>()), Times.Never);
        }
    }
}
