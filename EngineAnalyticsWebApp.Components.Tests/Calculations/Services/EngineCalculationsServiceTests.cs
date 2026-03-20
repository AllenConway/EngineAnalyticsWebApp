using EngineAnalyticsWebApp.Components.Calculations.Services;
using EngineAnalyticsWebApp.Shared.Models.Engine;

namespace EngineAnalyticsWebApp.Components.Tests.Calculations.Services
{
    public class EngineCalculationsServiceTests
    {
        private readonly EngineCalculationsService _sut = new();

        // --- CalculateEngineHorsepower ---

        [Fact]
        public void CalculateEngineHorsepower_WithValidInputs_ReturnsNonZeroHorsepower()
        {
            // Arrange
            var horsepower = new Horsepower { Weight = 3500, EstimatedTime = 12.5 };

            // Act
            var result = _sut.CalculateEngineHorsepower(horsepower);

            // Assert
            Assert.True(result.RearWheelHorsepower > 0);
            Assert.True(result.FlywheelHorsepower > 0);
        }

        [Fact]
        public void CalculateEngineHorsepower_WithValidInputs_FlywheelHorsepowerExceedsRearWheelHorsepower()
        {
            // Arrange
            var horsepower = new Horsepower { Weight = 3500, EstimatedTime = 12.5 };

            // Act
            var result = _sut.CalculateEngineHorsepower(horsepower);

            // Assert
            Assert.True(result.FlywheelHorsepower > result.RearWheelHorsepower);
        }

        [Fact]
        public void CalculateEngineHorsepower_WithValidInputs_FlywheelHorsepowerApplies1146DrivetrainMultiplier()
        {
            // Arrange
            var horsepower = new Horsepower { Weight = 3500, EstimatedTime = 12.5 };

            // Act
            var result = _sut.CalculateEngineHorsepower(horsepower);

            // Assert
            var expectedFlywheelHorsepower = Math.Round(result.RearWheelHorsepower * 1.146);
            Assert.Equal(expectedFlywheelHorsepower, result.FlywheelHorsepower);
        }

        [Fact]
        public void CalculateEngineHorsepower_WithNullWeight_ReturnsZeroHorsepower()
        {
            // Arrange
            var horsepower = new Horsepower { Weight = null, EstimatedTime = 12.5 };

            // Act
            var result = _sut.CalculateEngineHorsepower(horsepower);

            // Assert
            Assert.Equal(0, result.RearWheelHorsepower);
            Assert.Equal(0, result.FlywheelHorsepower);
        }

        [Fact]
        public void CalculateEngineHorsepower_WithNullEstimatedTime_ReturnsZeroHorsepower()
        {
            // Arrange
            var horsepower = new Horsepower { Weight = 3500, EstimatedTime = null };

            // Act
            var result = _sut.CalculateEngineHorsepower(horsepower);

            // Assert
            Assert.Equal(0, result.RearWheelHorsepower);
            Assert.Equal(0, result.FlywheelHorsepower);
        }

        [Fact]
        public void CalculateEngineHorsepower_WithAllNullInputs_ReturnsZeroHorsepower()
        {
            // Arrange
            var horsepower = new Horsepower { Weight = null, EstimatedTime = null };

            // Act
            var result = _sut.CalculateEngineHorsepower(horsepower);

            // Assert
            Assert.Equal(0, result.RearWheelHorsepower);
            Assert.Equal(0, result.FlywheelHorsepower);
        }

        // --- CalculateEngineDisplacement ---

        [Fact]
        public void CalculateEngineDisplacement_WithValidInputs_ReturnsCorrectDisplacement()
        {
            // Arrange
            var displacement = new Displacement { BoreSize = 4.0, CrankshaftStrokeLength = 3.48, Cylinders = 8 };
            var expectedDisplacement = Math.Round(Math.Pow(4.0 / 2.0, 2) * Math.PI * 3.48 * 8);

            // Act
            var result = _sut.CalculateEngineDisplacement(displacement);

            // Assert
            Assert.Equal(expectedDisplacement, result.Displacement);
        }

        [Fact]
        public void CalculateEngineDisplacement_WithNullBoreSize_ReturnsZeroDisplacement()
        {
            // Arrange
            var displacement = new Displacement { BoreSize = null, CrankshaftStrokeLength = 3.48, Cylinders = 8 };

            // Act
            var result = _sut.CalculateEngineDisplacement(displacement);

            // Assert
            Assert.Equal(0, result.Displacement);
        }

        [Fact]
        public void CalculateEngineDisplacement_WithNullCrankshaftStrokeLength_ReturnsZeroDisplacement()
        {
            // Arrange
            var displacement = new Displacement { BoreSize = 4.0, CrankshaftStrokeLength = null, Cylinders = 8 };

            // Act
            var result = _sut.CalculateEngineDisplacement(displacement);

            // Assert
            Assert.Equal(0, result.Displacement);
        }

        [Fact]
        public void CalculateEngineDisplacement_WithNullCylinders_ReturnsZeroDisplacement()
        {
            // Arrange
            var displacement = new Displacement { BoreSize = 4.0, CrankshaftStrokeLength = 3.48, Cylinders = null };

            // Act
            var result = _sut.CalculateEngineDisplacement(displacement);

            // Assert
            Assert.Equal(0, result.Displacement);
        }

        [Fact]
        public void CalculateEngineDisplacement_WithAllNullInputs_ReturnsZeroDisplacement()
        {
            // Arrange
            var displacement = new Displacement { BoreSize = null, CrankshaftStrokeLength = null, Cylinders = null };

            // Act
            var result = _sut.CalculateEngineDisplacement(displacement);

            // Assert
            Assert.Equal(0, result.Displacement);
        }

        // --- CalculateEngineTorque ---

        [Fact]
        public void CalculateEngineTorque_WithValidInputs_ReturnsCorrectTorque()
        {
            // Arrange
            var torque = new Torque { Horsepower = 400, EngineRPM = 5000 };
            var expectedTorque = Math.Round((400.0 * 5252) / 5000.0);

            // Act
            var result = _sut.CalculateEngineTorque(torque);

            // Assert
            Assert.Equal(expectedTorque, result.Torque);
        }

        [Fact]
        public void CalculateEngineTorque_WithNullHorsepower_ReturnsZeroTorque()
        {
            // Arrange
            var torque = new Torque { Horsepower = null, EngineRPM = 5000 };

            // Act
            var result = _sut.CalculateEngineTorque(torque);

            // Assert
            Assert.Equal(0, result.Torque);
        }

        [Fact]
        public void CalculateEngineTorque_WithNullEngineRPM_ReturnsZeroTorque()
        {
            // Arrange
            var torque = new Torque { Horsepower = 400, EngineRPM = null };

            // Act
            var result = _sut.CalculateEngineTorque(torque);

            // Assert
            Assert.Equal(0, result.Torque);
        }

        [Fact]
        public void CalculateEngineTorque_WithAllNullInputs_ReturnsZeroTorque()
        {
            // Arrange
            var torque = new Torque { Horsepower = null, EngineRPM = null };

            // Act
            var result = _sut.CalculateEngineTorque(torque);

            // Assert
            Assert.Equal(0, result.Torque);
        }
    }
}
