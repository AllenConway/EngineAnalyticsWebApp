using Bunit;
using EngineAnalyticsWebApp.Components.Filters;
using Microsoft.AspNetCore.Components;

namespace EngineAnalyticsWebApp.Components.Tests.Filters
{
    public class HorsepowerFilterComponentTests : BunitContext
    {
        [Fact]
        public void HorsepowerFilter_RendersFilterDescription()
        {
            // Arrange
            // Act
            var cut = Render<HorsepowerFilter>(parameters => parameters
                .Add(p => p.FilterDescription, "Model"));

            // Assert
            Assert.Contains("Model", cut.Find("label").TextContent);
        }

        [Fact]
        public void HorsepowerFilter_RendersInputAndButtons()
        {
            // Arrange
            // Act
            var cut = Render<HorsepowerFilter>(parameters => parameters
                .Add(p => p.FilterDescription, "Model"));

            // Assert
            Assert.NotNull(cut.Find("#filterInput"));
            Assert.NotNull(cut.Find("button.btn-primary"));
            Assert.NotNull(cut.Find("button.btn-secondary"));
        }

        [Fact]
        public void HorsepowerFilter_WhenFilterApplied_InvokesCallbackWithValue()
        {
            // Arrange
            string? appliedFilter = null;
            var cut = Render<HorsepowerFilter>(parameters => parameters
                .Add(p => p.FilterDescription, "Model")
                .Add(p => p.OnFilterApplied, (string value) => appliedFilter = value));

            // Act
            cut.Find("#filterInput").Change("Mustang");
            cut.Find("button.btn-primary").Click();

            // Assert
            Assert.Equal("Mustang", appliedFilter);
        }

        [Fact]
        public void HorsepowerFilter_WhenEmptyFilterApplied_DoesNotInvokeCallback()
        {
            // Arrange
            var callbackInvoked = false;
            var cut = Render<HorsepowerFilter>(parameters => parameters
                .Add(p => p.FilterDescription, "Model")
                .Add(p => p.OnFilterApplied, (string value) => callbackInvoked = true));

            // Act
            cut.Find("button.btn-primary").Click();

            // Assert
            Assert.False(callbackInvoked);
        }

        [Fact]
        public void HorsepowerFilter_WhenFilterCleared_InvokesCallbackWithNull()
        {
            // Arrange
            string? appliedFilter = "initial";
            var cut = Render<HorsepowerFilter>(parameters => parameters
                .Add(p => p.FilterDescription, "Model")
                .Add(p => p.OnFilterApplied, (string value) => appliedFilter = value));

            cut.Find("#filterInput").Change("Mustang");
            cut.Find("button.btn-primary").Click();

            // Act
            cut.Find("button.btn-secondary").Click();

            // Assert
            Assert.Null(appliedFilter);
        }
    }
}
