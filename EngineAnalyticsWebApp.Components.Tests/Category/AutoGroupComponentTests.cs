using Bunit;
using EngineAnalyticsWebApp.Components.Category;
using EngineAnalyticsWebApp.Shared.Models.Engine;
using Microsoft.AspNetCore.Components;

namespace EngineAnalyticsWebApp.Components.Tests.Category
{
    public class AutoGroupComponentTests : BunitContext
    {
        private static RenderFragment CascadingAuto(Automobile auto, RenderFragment child) => builder =>
        {
            builder.OpenComponent<CascadingValue<Automobile>>(0);
            builder.AddAttribute(1, "Value", auto);
            builder.AddAttribute(2, "ChildContent", child);
            builder.CloseComponent();
        };

        [Fact]
        public void AutoGroup_RendersYearMakeModelInputs()
        {
            // Arrange
            var auto = new Automobile { Year = 2023, Make = "Ford", Model = "Mustang" };

            // Act
            var cut = Render<AutoGroup>(parameters => parameters
                .AddCascadingValue(auto));

            // Assert
            Assert.NotNull(cut.Find("#vehicleYear"));
            Assert.NotNull(cut.Find("#vehicleMake"));
            Assert.NotNull(cut.Find("#vehicleModel"));
        }

        [Fact]
        public void AutoGroup_BindsMakeValue()
        {
            // Arrange
            var auto = new Automobile { Year = 2023, Make = "Ford", Model = "Mustang" };

            // Act
            var cut = Render<AutoGroup>(parameters => parameters
                .AddCascadingValue(auto));

            // Assert
            var makeInput = cut.Find("#vehicleMake");
            Assert.Equal("Ford", makeInput.GetAttribute("value"));
        }

        [Fact]
        public void AutoGroup_WhenMakeChanged_UpdatesCascadedAutomobile()
        {
            // Arrange
            var auto = new Automobile { Year = 2023, Make = "Ford", Model = "Mustang" };
            var cut = Render<AutoGroup>(parameters => parameters
                .AddCascadingValue(auto));

            // Act
            cut.Find("#vehicleModel").Change("GT500");

            // Assert
            Assert.Equal("GT500", auto.Model);
        }
    }
}
