using System.ComponentModel.DataAnnotations;
using EngineAnalyticsWebApp.Shared.Models.Engine;

namespace EngineAnalyticsWebApp.Shared.Tests.Models.Engine
{
    public class AutomobileTests
    {
        [Fact]
        public void Automobile_WithAllRequiredFields_IsValid()
        {
            // Arrange
            var automobile = new Automobile
            {
                Year = 2023,
                Make = "Ford",
                Model = "Mustang"
            };

            // Act
            var results = ValidateModel(automobile);

            // Assert
            Assert.Empty(results);
        }

        [Fact]
        public void Automobile_WithNullYear_FailsValidation()
        {
            // Arrange
            var automobile = new Automobile
            {
                Year = null,
                Make = "Ford",
                Model = "Mustang"
            };

            // Act
            var results = ValidateModel(automobile);

            // Assert
            Assert.Contains(results, v => v.MemberNames.Contains("Year"));
        }

        [Fact]
        public void Automobile_WithNullMake_FailsValidation()
        {
            // Arrange
            var automobile = new Automobile
            {
                Year = 2023,
                Make = null,
                Model = "Mustang"
            };

            // Act
            var results = ValidateModel(automobile);

            // Assert
            Assert.Contains(results, v => v.MemberNames.Contains("Make"));
        }

        [Fact]
        public void Automobile_WithNullModel_FailsValidation()
        {
            // Arrange
            var automobile = new Automobile
            {
                Year = 2023,
                Make = "Ford",
                Model = null
            };

            // Act
            var results = ValidateModel(automobile);

            // Assert
            Assert.Contains(results, v => v.MemberNames.Contains("Model"));
        }

        [Fact]
        public void Automobile_OptionalProperties_DefaultToNull()
        {
            // Arrange
            // Act
            var automobile = new Automobile();

            // Assert
            Assert.Null(automobile.Horsepower);
            Assert.Null(automobile.Displacement);
            Assert.Null(automobile.Torque);
            Assert.Null(automobile.EngineAnalytics);
        }

        [Fact]
        public void Automobile_CanSetOptionalProperties()
        {
            // Arrange
            var hp = new Horsepower { Weight = 3500, EstimatedTime = 12.5 };
            var disp = new Displacement { BoreSize = 4.0, CrankshaftStrokeLength = 3.48, Cylinders = 8 };
            var torque = new Torque { Horsepower = 400, EngineRPM = 5000 };
            var analytics = new EngineAnalytics { RearWheelHorsepower = 300 };

            // Act
            var automobile = new Automobile
            {
                Year = 2023,
                Make = "Ford",
                Model = "Mustang",
                Horsepower = hp,
                Displacement = disp,
                Torque = torque,
                EngineAnalytics = analytics
            };

            // Assert
            Assert.Same(hp, automobile.Horsepower);
            Assert.Same(disp, automobile.Displacement);
            Assert.Same(torque, automobile.Torque);
            Assert.Same(analytics, automobile.EngineAnalytics);
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
