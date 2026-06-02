using EngineAnalyticsWebApp.Shared.Models.Engine;

namespace EngineAnalyticsWebApp.Shared.Tests.Models.Engine
{
    public class EngineAnalyticsModelTests
    {
        [Fact]
        public void EngineAnalytics_DefaultValues_AreZero()
        {
            // Arrange
            // Act
            var analytics = new EngineAnalytics();

            // Assert
            Assert.Equal(0, analytics.RearWheelHorsepower);
            Assert.Equal(0, analytics.FlywheelHorsepower);
            Assert.Equal(0, analytics.Displacement);
            Assert.Equal(0, analytics.Torque);
        }

        [Fact]
        public void EngineAnalytics_CanSetAllProperties()
        {
            // Arrange
            // Act
            var analytics = new EngineAnalytics
            {
                RearWheelHorsepower = 300,
                FlywheelHorsepower = 344,
                Displacement = 350,
                Torque = 420
            };

            // Assert
            Assert.Equal(300, analytics.RearWheelHorsepower);
            Assert.Equal(344, analytics.FlywheelHorsepower);
            Assert.Equal(350, analytics.Displacement);
            Assert.Equal(420, analytics.Torque);
        }
    }
}
