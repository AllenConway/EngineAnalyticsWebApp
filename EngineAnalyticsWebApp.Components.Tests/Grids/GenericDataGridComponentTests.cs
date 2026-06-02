using Bunit;
using EngineAnalyticsWebApp.Components.Grids;
using EngineAnalyticsWebApp.Shared.Models.Engine;
using Microsoft.AspNetCore.Components;

namespace EngineAnalyticsWebApp.Components.Tests.Grids
{
    public class GenericDataGridComponentTests : BlazoriseComponentTestContext
    {
        private static RenderFragment Columns() => builder =>
        {
            // A minimal DataGridColumn for the Model field
            builder.OpenComponent(0, typeof(Blazorise.DataGrid.DataGridColumn<Automobile>));
            builder.AddAttribute(1, "Field", nameof(Automobile.Model));
            builder.AddAttribute(2, "Caption", "Model");
            builder.CloseComponent();
        };

        [Fact]
        public void GenericDataGrid_WithData_RendersGrid()
        {
            // Arrange
            var data = new List<Automobile>
            {
                new() { Year = 2023, Make = "Ford", Model = "Mustang" }
            };

            // Act
            var cut = Render<GenericDataGrid<Automobile>>(parameters => parameters
                .Add(p => p.Data, data)
                .Add(p => p.Columns, Columns()));

            // Assert
            cut.WaitForAssertion(() => Assert.Contains("Mustang", cut.Markup));
        }

        [Fact]
        public void GenericDataGrid_WithEmptyData_RendersWithoutError()
        {
            // Arrange
            // Act
            var cut = Render<GenericDataGrid<Automobile>>(parameters => parameters
                .Add(p => p.Data, new List<Automobile>())
                .Add(p => p.Columns, Columns()));

            // Assert
            cut.WaitForAssertion(() => Assert.NotNull(cut));
        }
    }
}
