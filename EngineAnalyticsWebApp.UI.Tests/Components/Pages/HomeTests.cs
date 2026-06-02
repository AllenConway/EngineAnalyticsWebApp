using Bunit;
using EngineAnalyticsWebApp.UI.Components.Pages;

namespace EngineAnalyticsWebApp.UI.Tests.Components.Pages
{
    public class HomeTests : BunitContext
    {
        [Fact]
        public void Home_RendersWelcomeTitle()
        {
            // Arrange
            // Act
            var cut = Render<Home>();

            // Assert
            Assert.Contains("Welcome to the Engine Analytics Blazor App", cut.Markup);
        }

        [Fact]
        public void Home_RendersInstructionText()
        {
            // Arrange
            // Act
            var cut = Render<Home>();

            // Assert
            Assert.Contains("Select an option from the menu", cut.Markup);
        }

        [Fact]
        public void Home_RendersPerformanceImage()
        {
            // Arrange
            // Act
            var cut = Render<Home>();

            // Assert
            var img = cut.Find("img");
            Assert.Equal("images/performance.jpg", img.GetAttribute("src"));
        }
    }
}
