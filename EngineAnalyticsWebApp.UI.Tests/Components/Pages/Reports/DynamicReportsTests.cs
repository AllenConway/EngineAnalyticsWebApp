using Bunit;
using EngineAnalyticsWebApp.Shared.Models.Engine;
using EngineAnalyticsWebApp.Shared.Services.Data;
using EngineAnalyticsWebApp.UI.Components.Pages.Reports;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace EngineAnalyticsWebApp.UI.Tests.Components.Pages.Reports
{
    public class DynamicReportsTests : BlazoriseTestContext
    {
        private readonly Mock<IAutomobileDataService> _automobileDataServiceMock;

        public DynamicReportsTests()
        {
            _automobileDataServiceMock = new Mock<IAutomobileDataService>();
            _automobileDataServiceMock.Setup(s => s.GetAutomobiles()).ReturnsAsync(new List<Automobile>());
            Services.AddSingleton(_automobileDataServiceMock.Object);
        }

        [Fact]
        public void DynamicReports_RendersReportSelector()
        {
            // Arrange
            // Act
            var cut = Render<DynamicReports>();

            // Assert
            Assert.NotNull(cut.Find("select"));
            Assert.Contains("Select a report to view", cut.Markup);
        }

        [Fact]
        public void DynamicReports_InitiallyShowsNoDynamicComponent()
        {
            // Arrange
            // Act
            var cut = Render<DynamicReports>();

            // Assert
            // No report selected so the data service should not have been queried yet
            _automobileDataServiceMock.Verify(s => s.GetAutomobiles(), Times.Never);
        }

        [Fact]
        public void DynamicReports_WhenDisplacementSelected_RendersDisplacementReport()
        {
            // Arrange
            var cut = Render<DynamicReports>();

            // Act
            cut.Find("select").Change("Displacement");

            // Assert
            cut.WaitForAssertion(() =>
                _automobileDataServiceMock.Verify(s => s.GetAutomobiles(), Times.AtLeastOnce));
        }

        [Fact]
        public void DynamicReports_WhenTorqueSelected_RendersTorqueReport()
        {
            // Arrange
            var cut = Render<DynamicReports>();

            // Act
            cut.Find("select").Change("Torque");

            // Assert
            cut.WaitForAssertion(() =>
                _automobileDataServiceMock.Verify(s => s.GetAutomobiles(), Times.AtLeastOnce));
        }

        [Fact]
        public void DynamicReports_WhenEmptySelected_RendersNoReport()
        {
            // Arrange
            var cut = Render<DynamicReports>();

            // Act
            cut.Find("select").Change("");

            // Assert
            _automobileDataServiceMock.Verify(s => s.GetAutomobiles(), Times.Never);
        }
    }
}
