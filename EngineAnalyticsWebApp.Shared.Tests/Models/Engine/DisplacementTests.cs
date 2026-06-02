using System.ComponentModel.DataAnnotations;
using EngineAnalyticsWebApp.Shared.Models.Engine;

namespace EngineAnalyticsWebApp.Shared.Tests.Models.Engine
{
    public class DisplacementTests
    {
        [Fact]
        public void Displacement_WithValidValues_IsValid()
        {
            // Arrange
            var disp = new Displacement { BoreSize = 4.0, CrankshaftStrokeLength = 3.48, Cylinders = 8 };

            // Act
            var results = ValidateModel(disp);

            // Assert
            Assert.Empty(results);
        }

        [Fact]
        public void Displacement_WithNullBoreSize_FailsValidation()
        {
            // Arrange
            var disp = new Displacement { BoreSize = null, CrankshaftStrokeLength = 3.48, Cylinders = 8 };

            // Act
            var results = ValidateModel(disp);

            // Assert
            Assert.Contains(results, v => v.MemberNames.Contains("BoreSize"));
        }

        [Fact]
        public void Displacement_WithNullCrankshaftStrokeLength_FailsValidation()
        {
            // Arrange
            var disp = new Displacement { BoreSize = 4.0, CrankshaftStrokeLength = null, Cylinders = 8 };

            // Act
            var results = ValidateModel(disp);

            // Assert
            Assert.Contains(results, v => v.MemberNames.Contains("CrankshaftStrokeLength"));
        }

        [Fact]
        public void Displacement_WithNullCylinders_FailsValidation()
        {
            // Arrange
            var disp = new Displacement { BoreSize = 4.0, CrankshaftStrokeLength = 3.48, Cylinders = null };

            // Act
            var results = ValidateModel(disp);

            // Assert
            Assert.Contains(results, v => v.MemberNames.Contains("Cylinders"));
        }

        [Fact]
        public void Displacement_WithAllNullValues_FailsAllValidations()
        {
            // Arrange
            var disp = new Displacement { BoreSize = null, CrankshaftStrokeLength = null, Cylinders = null };

            // Act
            var results = ValidateModel(disp);

            // Assert
            Assert.Equal(3, results.Count);
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
