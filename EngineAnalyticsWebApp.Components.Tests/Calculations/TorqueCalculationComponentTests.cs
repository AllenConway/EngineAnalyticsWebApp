using Bunit;
using EngineAnalyticsWebApp.Components.Calculations;
using EngineAnalyticsWebApp.Components.Calculations.Services;
using EngineAnalyticsWebApp.Shared.Models.Engine;
using EngineAnalyticsWebApp.Shared.Services.Data;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace EngineAnalyticsWebApp.Components.Tests.Calculations
{
    public class TorqueCalculationComponentTests : TestContext
    {
        private readonly Mock<IAutomobileDataService> _automobileDataServiceMock;
        private readonly Mock<IEngineCalculationsService> _engineCalculationsServiceMock;

        public TorqueCalculationComponentTests()
        {
            _automobileDataServiceMock = new Mock<IAutomobileDataService>();
            _engineCalculationsServiceMock = new Mock<IEngineCalculationsService>();

            _automobileDataServiceMock
                .Setup(s => s.AddAutomobile(It.IsAny<Automobile>()))
                .Returns(Task.CompletedTask);

            _engineCalculationsServiceMock
                .Setup(s => s.CalculateEngineTorque(It.IsAny<Torque>()))
                .Returns(new EngineAnalytics { Torque = 420 });

            Services.AddSingleton(_automobileDataServiceMock.Object);
            Services.AddSingleton(_engineCalculationsServiceMock.Object);
        }

        [Fact]
        public void TorqueCalculation_RendersExpectedInputFields()
        {
            // Arrange
            // Act
            var cut = Render<TorqueCalculation>();

            // Assert
            Assert.NotNull(cut.Find("#engineHorsepower"));
            Assert.NotNull(cut.Find("#engineRPM"));
            Assert.NotNull(cut.Find("button[type='submit']"));
        }

        [Fact]
        public void TorqueCalculation_OnInvalidSubmit_DisplaysRequiredFieldsErrorMessage()
        {
            // Arrange
            var cut = Render<TorqueCalculation>();

            // Act
            cut.Find("button[type='submit']").Click();

            // Assert
            Assert.Contains("Required fields are missing", cut.Find(".alert").TextContent);
        }

        [Fact]
        public void TorqueCalculation_OnValidSubmit_DisplaysSuccessMessage()
        {
            // Arrange
            var cut = Render<TorqueCalculation>();

            cut.Find("#vehicleYear").Change("2023");
            cut.Find("#vehicleMake").Change("Ford");
            cut.Find("#vehicleModel").Change("Mustang");
            cut.Find("#engineHorsepower").Change("400");
            cut.Find("#engineRPM").Change("5000");

            // Act
            cut.Find("button[type='submit']").Click();

            // Assert
            cut.WaitForAssertion(() =>
                Assert.Contains("Successfully saved calculations", cut.Find(".alert").TextContent));
        }

        [Fact]
        public void TorqueCalculation_OnValidSubmit_InvokesCalculationServiceOnce()
        {
            // Arrange
            var cut = Render<TorqueCalculation>();

            cut.Find("#vehicleYear").Change("2023");
            cut.Find("#vehicleMake").Change("Ford");
            cut.Find("#vehicleModel").Change("Mustang");
            cut.Find("#engineHorsepower").Change("400");
            cut.Find("#engineRPM").Change("5000");

            // Act
            cut.Find("button[type='submit']").Click();

            // Assert
            cut.WaitForAssertion(() =>
                _engineCalculationsServiceMock.Verify(
                    s => s.CalculateEngineTorque(It.IsAny<Torque>()), Times.Once));
        }
    }
}
