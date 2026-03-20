using Bunit;
using EngineAnalyticsWebApp.Components.Calculations;
using EngineAnalyticsWebApp.Components.Calculations.Services;
using EngineAnalyticsWebApp.Shared.Models.Engine;
using EngineAnalyticsWebApp.Shared.Services.Data;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace EngineAnalyticsWebApp.Components.Tests.Calculations
{
    public class DisplacementCalculationComponentTests : TestContext
    {
        private readonly Mock<IAutomobileDataService> _automobileDataServiceMock;
        private readonly Mock<IEngineCalculationsService> _engineCalculationsServiceMock;

        public DisplacementCalculationComponentTests()
        {
            _automobileDataServiceMock = new Mock<IAutomobileDataService>();
            _engineCalculationsServiceMock = new Mock<IEngineCalculationsService>();

            _automobileDataServiceMock
                .Setup(s => s.AddAutomobile(It.IsAny<Automobile>()))
                .Returns(Task.CompletedTask);

            _engineCalculationsServiceMock
                .Setup(s => s.CalculateEngineDisplacement(It.IsAny<Displacement>()))
                .Returns(new EngineAnalytics { Displacement = 350 });

            Services.AddSingleton(_automobileDataServiceMock.Object);
            Services.AddSingleton(_engineCalculationsServiceMock.Object);
        }

        [Fact]
        public void DisplacementCalculation_RendersExpectedInputFields()
        {
            // Arrange
            // Act
            var cut = Render<DisplacementCalculation>();

            // Assert
            Assert.NotNull(cut.Find("#engineBoreSize"));
            Assert.NotNull(cut.Find("#engineCrankshaftStrokeLength"));
            Assert.NotNull(cut.Find("#engineCylinders"));
            Assert.NotNull(cut.Find("button[type='submit']"));
        }

        [Fact]
        public void DisplacementCalculation_OnInvalidSubmit_DisplaysRequiredFieldsErrorMessage()
        {
            // Arrange
            var cut = Render<DisplacementCalculation>();

            // Act
            cut.Find("button[type='submit']").Click();

            // Assert
            Assert.Contains("Required fields are missing", cut.Find(".alert").TextContent);
        }

        [Fact]
        public void DisplacementCalculation_OnValidSubmit_DisplaysSuccessMessage()
        {
            // Arrange
            var cut = Render<DisplacementCalculation>();

            cut.Find("#vehicleYear").Change("2023");
            cut.Find("#vehicleMake").Change("Ford");
            cut.Find("#vehicleModel").Change("Mustang");
            cut.Find("#engineBoreSize").Change("4.0");
            cut.Find("#engineCrankshaftStrokeLength").Change("3.48");
            cut.Find("#engineCylinders").Change("8");

            // Act
            cut.Find("button[type='submit']").Click();

            // Assert
            cut.WaitForAssertion(() =>
                Assert.Contains("Successfully saved calculations", cut.Find(".alert").TextContent));
        }

        [Fact]
        public void DisplacementCalculation_OnValidSubmit_InvokesCalculationServiceOnce()
        {
            // Arrange
            var cut = Render<DisplacementCalculation>();

            cut.Find("#vehicleYear").Change("2023");
            cut.Find("#vehicleMake").Change("Ford");
            cut.Find("#vehicleModel").Change("Mustang");
            cut.Find("#engineBoreSize").Change("4.0");
            cut.Find("#engineCrankshaftStrokeLength").Change("3.48");
            cut.Find("#engineCylinders").Change("8");

            // Act
            cut.Find("button[type='submit']").Click();

            // Assert
            cut.WaitForAssertion(() =>
                _engineCalculationsServiceMock.Verify(
                    s => s.CalculateEngineDisplacement(It.IsAny<Displacement>()), Times.Once));
        }
    }
}
