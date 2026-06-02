using System.ComponentModel.DataAnnotations;
using EngineAnalyticsWebApp.Shared.Models.Engine;

namespace EngineAnalyticsWebApp.Shared.Tests.Models.Engine
{
    public class TorqueTests
    {
        [Fact]
        public void Torque_WithValidValues_IsValid()
        {
            // Arrange
            var torque = new Torque { Horsepower = 400, EngineRPM = 5000 };

            // Act
            var results = ValidateModel(torque);

            // Assert
            Assert.Empty(results);
        }

        [Fact]
        public void Torque_WithNullHorsepower_FailsValidation()
        {
            // Arrange
            var torque = new Torque { Horsepower = null, EngineRPM = 5000 };

            // Act
            var results = ValidateModel(torque);

            // Assert
            Assert.Contains(results, v => v.MemberNames.Contains("Horsepower"));
        }

        [Fact]
        public void Torque_WithNullEngineRPM_FailsValidation()
        {
            // Arrange
            var torque = new Torque { Horsepower = 400, EngineRPM = null };

            // Act
            var results = ValidateModel(torque);

            // Assert
            Assert.Contains(results, v => v.MemberNames.Contains("EngineRPM"));
        }

        [Fact]
        public void Torque_WithAllNullValues_FailsAllValidations()
        {
            // Arrange
            var torque = new Torque { Horsepower = null, EngineRPM = null };

            // Act
            var results = ValidateModel(torque);

            // Assert
            Assert.Equal(2, results.Count);
        }

        private static List<ValidationResult> ValidateModel(object model)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(model);
            Validator.TryValidateObject(model, context, results, true);
            return results;
        }
    }
}
