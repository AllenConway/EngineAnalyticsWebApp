using Bunit;
using EngineAnalyticsWebApp.Components.Reporting;
using Microsoft.AspNetCore.Components;

namespace EngineAnalyticsWebApp.Components.Tests.Reporting
{
    public class ReportHeaderComponentTests : BunitContext
    {
        [Fact]
        public void ReportHeader_RendersTitle()
        {
            // Arrange
            // Act
            var cut = Render<ReportHeader>(parameters => parameters
                .Add(p => p.Title, "My Report")
                .Add(p => p.ChildContent, (RenderFragment)(builder =>
                    builder.AddMarkupContent(0, "<p>Body</p>"))));

            // Assert
            Assert.Contains("My Report", cut.Find("h3").TextContent);
        }

        [Fact]
        public void ReportHeader_RendersChildContent()
        {
            // Arrange
            // Act
            var cut = Render<ReportHeader>(parameters => parameters
                .Add(p => p.Title, "My Report")
                .Add(p => p.ChildContent, (RenderFragment)(builder =>
                    builder.AddMarkupContent(0, "<p>Some content here</p>"))));

            // Assert
            Assert.Contains("Some content here", cut.Markup);
        }

        [Fact]
        public void ReportHeader_AppliesTop20CssClassToTitle()
        {
            // Arrange
            // Act
            var cut = Render<ReportHeader>(parameters => parameters
                .Add(p => p.Title, "Styled")
                .Add(p => p.ChildContent, (RenderFragment)(builder => { })));

            // Assert
            Assert.Contains("top20", cut.Find("h3").ClassList);
        }
    }
}
