using Bunit;
using EngineAnalyticsWebApp.Components.Tables;
using EngineAnalyticsWebApp.Shared.Models.Engine;
using Microsoft.AspNetCore.Components;

namespace EngineAnalyticsWebApp.Components.Tests.Tables
{
    public class GenericTableComponentTests : BlazoriseComponentTestContext
    {
        private static RenderFragment HeaderCells() => builder =>
            builder.AddMarkupContent(0, "<th>Model</th>");

        private static RenderFragment<Automobile> RowCells() => auto => builder =>
            builder.AddMarkupContent(0, $"<td>{auto.Model}</td>");

        [Fact]
        public void GenericTable_WithItems_RendersRows()
        {
            // Arrange
            var items = new List<Automobile>
            {
                new() { Year = 2023, Make = "Ford", Model = "Mustang" },
                new() { Year = 2000, Make = "Honda", Model = "Civic" }
            };

            // Act
            var cut = Render<GenericTable<Automobile>>(parameters => parameters
                .Add(p => p.Items, items)
                .Add(p => p.HeaderCells, HeaderCells())
                .Add(p => p.RowCells, RowCells()));

            // Assert
            cut.WaitForAssertion(() => Assert.Contains("Mustang", cut.Markup));
            Assert.Contains("Civic", cut.Markup);
        }

        [Fact]
        public void GenericTable_WithNoItems_RendersHeaderOnly()
        {
            // Arrange
            // Act
            var cut = Render<GenericTable<Automobile>>(parameters => parameters
                .Add(p => p.Items, new List<Automobile>())
                .Add(p => p.HeaderCells, HeaderCells())
                .Add(p => p.RowCells, RowCells()));

            // Assert
            cut.WaitForAssertion(() => Assert.Contains("Model", cut.Markup));
        }

        [Fact]
        public void GenericTable_RendersHeaderCells()
        {
            // Arrange
            var items = new List<Automobile>
            {
                new() { Year = 2023, Make = "Ford", Model = "Mustang" }
            };

            // Act
            var cut = Render<GenericTable<Automobile>>(parameters => parameters
                .Add(p => p.Items, items)
                .Add(p => p.HeaderCells, HeaderCells())
                .Add(p => p.RowCells, RowCells()));

            // Assert
            cut.WaitForAssertion(() => Assert.Contains("Model", cut.Markup));
        }
    }
}
