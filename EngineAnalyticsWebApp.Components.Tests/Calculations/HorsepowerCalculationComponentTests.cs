using Bunit;
using EngineAnalyticsWebApp.Components.Calculations;
using EngineAnalyticsWebApp.Components.Calculations.Services;
using EngineAnalyticsWebApp.Shared.Models.Engine;
using EngineAnalyticsWebApp.Shared.Services.Data;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace EngineAnalyticsWebApp.Components.Tests.Calculations
{
    public class HorsepowerCalculationComponentTests : TestContext
    {
        private readonly Mock<IAutomobileDataService> _automobileDataServiceMock;
        private readonly Mock<IEngineCalculationsService> _engineCalculationsServiceMock;

        public HorsepowerCalculationComponentTests()
        {
            _automobileDataServiceMock = new Mock<IAutomobileDataService>();
            _engineCalculationsServiceMock = new Mock<IEngineCalculationsService>();

            _automobileDataServiceMock
                .Setup(s => s.AddAutomobile(It.IsAny<Automobile>()))
                .Returns(Task.CompletedTask);

            _engineCalculationsServiceMock
                .Setup(s => s.CalculateEngineHorsepower(It.IsAny<Horsepower>()))
                .Returns(new EngineAnalytics { RearWheelHorsepower = 354, FlywheelHorsepower = 406 });

            Services.AddSingleton(_automobileDataServiceMock.Object);
            Services.AddSingleton(_engineCalculationsServiceMock.Object);
        }

        [Fact]
        public void HorsepowerCalculation_RendersExpectedInputFields()
        {
            // Arrange
            // Act
            var cut = Render<HorsepowerCalculation>();

            // Assert
            Assert.NotNull(cut.Find("#vehicleWeight"));
            Assert.NotNull(cut.Find("#estimatedTime"));
            Assert.NotNull(cut.Find("button[type='submit']"));
        }

        [Fact]
        public void HorsepowerCalculation_OnInvalidSubmit_DisplaysRequiredFieldsErrorMessage()
        {
            // Arrange
            var cut = Render<HorsepowerCalculation>();

            // Act
            cut.Find("button[type='submit']").Click();

            // Assert
            Assert.Contains("Required fields are missing", cut.Find(".alert").TextContent);
        }

        [Fact]
        public void HorsepowerCalculation_OnValidSubmit_DisplaysSuccessMessage()
        {
            // Arrange
            var cut = Render<HorsepowerCalculation>();

            cut.Find("#vehicleYear").Change("2023");
            cut.Find("#vehicleMake").Change("Ford");
            cut.Find("#vehicleModel").Change("Mustang");
            cut.Find("#vehicleWeight").Change("3500");
            cut.Find("#estimatedTime").Change("12.5");

            // Act
            cut.Find("button[type='submit']").Click();

            // Assert
            cut.WaitForAssertion(() =>
                Assert.Contains("Successfully saved calculations", cut.Find(".alert").TextContent));
        }

        [Fact]
        public void HorsepowerCalculation_OnValidSubmit_InvokesCalculationServiceOnce()
        {
            // Arrange
            var cut = Render<HorsepowerCalculation>();

            cut.Find("#vehicleYear").Change("2023");
            cut.Find("#vehicleMake").Change("Ford");
            cut.Find("#vehicleModel").Change("Mustang");
            cut.Find("#vehicleWeight").Change("3500");
            cut.Find("#estimatedTime").Change("12.5");

            // Act
            cut.Find("button[type='submit']").Click();

            // Assert
            cut.WaitForAssertion(() =>
                _engineCalculationsServiceMock.Verify(
                    s => s.CalculateEngineHorsepower(It.IsAny<Horsepower>()), Times.Once));
        }
    }
}
