using Bunit;
using EngineAnalyticsWebApp.Components.Calculations.Services;
using EngineAnalyticsWebApp.Shared.Services.Data;
using EngineAnalyticsWebApp.UI.Components.Pages.Engine;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace EngineAnalyticsWebApp.UI.Tests.Components.Pages.Engine
{
    public class CalculateDisplacementTests : BunitContext
    {
        public CalculateDisplacementTests()
        {
            // Arrange: register the services the child DisplacementCalculation component depends on
            Services.AddSingleton(new Mock<IAutomobileDataService>().Object);
            Services.AddSingleton(new Mock<IEngineCalculationsService>().Object);
        }

        [Fact]
        public void CalculateDisplacement_RendersTitle()
        {
            // Arrange
            // Act
            var cut = Render<CalculateDisplacement>();

            // Assert
            Assert.Contains("Engine Displacement Calculation", cut.Markup);
        }

        [Fact]
        public void CalculateDisplacement_RendersDisplacementCalculationComponent()
        {
            // Arrange
            // Act
            var cut = Render<CalculateDisplacement>();

            // Assert
            Assert.NotNull(cut.Find("#engineBoreSize"));
            Assert.NotNull(cut.Find("#engineCrankshaftStrokeLength"));
            Assert.NotNull(cut.Find("#engineCylinders"));
        }
    }
}
