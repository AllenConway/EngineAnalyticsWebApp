using Bunit;
using EngineAnalyticsWebApp.UI.Components.Layout;

namespace EngineAnalyticsWebApp.UI.Tests.Components.Layout
{
    public class NavMenuTests : BunitContext
    {
        [Fact]
        public void NavMenu_RendersBrand()
        {
            // Arrange
            // Act
            var cut = Render<NavMenu>();

            // Assert
            Assert.Contains("Engine Analytics App", cut.Markup);
        }

        [Fact]
        public void NavMenu_RendersNavigationLinks()
        {
            // Arrange
            // Act
            var cut = Render<NavMenu>();

            // Assert
            Assert.Contains("/engine/calculate-horsepower", cut.Markup);
            Assert.Contains("/engine/calculate-displacement", cut.Markup);
            Assert.Contains("/engine/calculate-torque", cut.Markup);
            Assert.Contains("/horsepower-results", cut.Markup);
            Assert.Contains("/torque-results", cut.Markup);
            Assert.Contains("/displacement-results", cut.Markup);
            Assert.Contains("/dynamic-reports", cut.Markup);
        }

        [Fact]
        public void NavMenu_InitiallyCollapsed()
        {
            // Arrange
            // Act
            var cut = Render<NavMenu>();

            // Assert
            Assert.Contains("sidebar-collapse", cut.Markup);
        }

        [Fact]
        public void NavMenu_WhenToggleClicked_TogglesCollapseState()
        {
            // Arrange
            var cut = Render<NavMenu>();

            // Act
            cut.Find("button.navbar-toggler").Click();

            // Assert
            Assert.DoesNotContain("sidebar-collapse", cut.Markup);
        }

        [Fact]
        public void NavMenu_WhenToggleClickedTwice_ReturnsToCollapsedState()
        {
            // Arrange
            var cut = Render<NavMenu>();

            // Act
            cut.Find("button.navbar-toggler").Click();
            cut.Find("button.navbar-toggler").Click();

            // Assert
            Assert.Contains("sidebar-collapse", cut.Markup);
        }
    }
}
