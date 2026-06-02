using System.ComponentModel.DataAnnotations;
using EngineAnalyticsWebApp.Shared.Models.Engine;

namespace EngineAnalyticsWebApp.Shared.Tests.Models.Engine
{
    public class HorsepowerTests
    {
        [Fact]
        public void Horsepower_WithValidValues_IsValid()
        {
            // Arrange
            var hp = new Horsepower { Weight = 3500, EstimatedTime = 12.5 };

            // Act
            var results = ValidateModel(hp);

            // Assert
            Assert.Empty(results);
        }

        [Fact]
        public void Horsepower_WithNullWeight_FailsValidation()
        {
            // Arrange
            var hp = new Horsepower { Weight = null, EstimatedTime = 12.5 };

            // Act
            var results = ValidateModel(hp);

            // Assert
            Assert.Contains(results, v => v.MemberNames.Contains("Weight"));
        }

        [Fact]
        public void Horsepower_WithNullEstimatedTime_FailsValidation()
        {
            // Arrange
            var hp = new Horsepower { Weight = 3500, EstimatedTime = null };

            // Act
            var results = ValidateModel(hp);

            // Assert
            Assert.Contains(results, v => v.MemberNames.Contains("EstimatedTime"));
        }

        [Fact]
        public void Horsepower_WithWeightBelowRange_FailsValidation()
        {
            // Arrange
            var hp = new Horsepower { Weight = 0, EstimatedTime = 12.5 };

            // Act
            var results = ValidateModel(hp);

            // Assert
            Assert.Contains(results, v => v.MemberNames.Contains("Weight"));
        }

        [Fact]
        public void Horsepower_WithWeightAboveRange_FailsValidation()
        {
            // Arrange
            var hp = new Horsepower { Weight = 10001, EstimatedTime = 12.5 };

            // Act
            var results = ValidateModel(hp);

            // Assert
            Assert.Contains(results, v => v.MemberNames.Contains("Weight"));
        }

        [Fact]
        public void Horsepower_WithEstimatedTimeBelowRange_FailsValidation()
        {
            // Arrange
            var hp = new Horsepower { Weight = 3500, EstimatedTime = 2 };

            // Act
            var results = ValidateModel(hp);

            // Assert
            Assert.Contains(results, v => v.MemberNames.Contains("EstimatedTime"));
        }

        [Fact]
        public void Horsepower_WithEstimatedTimeAboveRange_FailsValidation()
        {
            // Arrange
            var hp = new Horsepower { Weight = 3500, EstimatedTime = 26 };

            // Act
            var results = ValidateModel(hp);

            // Assert
            Assert.Contains(results, v => v.MemberNames.Contains("EstimatedTime"));
        }

        [Fact]
        public void Horsepower_WithBoundaryMinWeight_IsValid()
        {
            // Arrange
            var hp = new Horsepower { Weight = 1, EstimatedTime = 10 };

            // Act
            var results = ValidateModel(hp);

            // Assert
            Assert.Empty(results);
        }

        [Fact]
        public void Horsepower_WithBoundaryMaxWeight_IsValid()
        {
            // Arrange
            var hp = new Horsepower { Weight = 10000, EstimatedTime = 10 };

            // Act
            var results = ValidateModel(hp);

            // Assert
            Assert.Empty(results);
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
