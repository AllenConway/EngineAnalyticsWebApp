using Bunit;
using EngineAnalyticsWebApp.Components.Calculations.Services;
using EngineAnalyticsWebApp.Shared.Models.Engine;
using EngineAnalyticsWebApp.Shared.Services.Data;
using EngineAnalyticsWebApp.UI.Components.Pages.Engine;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace EngineAnalyticsWebApp.UI.Tests.Components.Pages.Engine
{
    public class CalculateHorsepowerTests : BunitContext
    {
        public CalculateHorsepowerTests()
        {
            // Arrange: register the services the child HorsepowerCalculation component depends on
            Services.AddSingleton(new Mock<IAutomobileDataService>().Object);
            Services.AddSingleton(new Mock<IEngineCalculationsService>().Object);
        }

        [Fact]
        public void CalculateHorsepower_RendersTitle()
        {
            // Arrange
            // Act
            var cut = Render<CalculateHorsepower>();

            // Assert
            Assert.Contains("Engine Horsepower Calculation", cut.Markup);
        }

        [Fact]
        public void CalculateHorsepower_RendersHorsepowerCalculationComponent()
        {
            // Arrange
            // Act
            var cut = Render<CalculateHorsepower>();

            // Assert
            Assert.NotNull(cut.Find("#vehicleWeight"));
            Assert.NotNull(cut.Find("#estimatedTime"));
        }
    }
}
