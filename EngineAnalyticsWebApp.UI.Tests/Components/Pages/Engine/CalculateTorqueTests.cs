using Bunit;
using EngineAnalyticsWebApp.Components.Calculations.Services;
using EngineAnalyticsWebApp.Shared.Services.Data;
using EngineAnalyticsWebApp.UI.Components.Pages.Engine;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace EngineAnalyticsWebApp.UI.Tests.Components.Pages.Engine
{
    public class CalculateTorqueTests : BunitContext
    {
        public CalculateTorqueTests()
        {
            // Arrange: register the services the child TorqueCalculation component depends on
            Services.AddSingleton(new Mock<IAutomobileDataService>().Object);
            Services.AddSingleton(new Mock<IEngineCalculationsService>().Object);
        }

        [Fact]
        public void CalculateTorque_RendersTitle()
        {
            // Arrange
            // Act
            var cut = Render<CalculateTorque>();

            // Assert
            Assert.Contains("Engine Torque Calculation", cut.Markup);
        }

        [Fact]
        public void CalculateTorque_RendersTorqueCalculationComponent()
        {
            // Arrange
            // Act
            var cut = Render<CalculateTorque>();

            // Assert
            Assert.NotNull(cut.Find("#engineHorsepower"));
            Assert.NotNull(cut.Find("#engineRPM"));
        }
    }
}
