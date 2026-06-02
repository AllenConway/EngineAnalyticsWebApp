using Bunit;
using EngineAnalyticsWebApp.Shared.Models.Engine;
using EngineAnalyticsWebApp.Shared.Services.Data;
using EngineAnalyticsWebApp.UI.Components.Pages.Reports;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace EngineAnalyticsWebApp.UI.Tests.Components.Pages.Reports
{
    public class DisplacementResultsTests : BlazoriseTestContext
    {
        private readonly Mock<IAutomobileDataService> _automobileDataServiceMock;

        public DisplacementResultsTests()
        {
            _automobileDataServiceMock = new Mock<IAutomobileDataService>();
            Services.AddSingleton(_automobileDataServiceMock.Object);
        }

        [Fact]
        public void DisplacementResults_RendersReportHeaderTitle()
        {
            // Arrange
            _automobileDataServiceMock.Setup(s => s.GetAutomobiles()).ReturnsAsync(new List<Automobile>());

            // Act
            var cut = Render<DisplacementResults>();

            // Assert
            Assert.Contains("Displacement Results", cut.Markup);
        }

        [Fact]
        public void DisplacementResults_RendersDescription()
        {
            // Arrange
            _automobileDataServiceMock.Setup(s => s.GetAutomobiles()).ReturnsAsync(new List<Automobile>());

            // Act
            var cut = Render<DisplacementResults>();

            // Assert
            Assert.Contains("logged engine displacement calculation results", cut.Markup);
        }

        [Fact]
        public void DisplacementResults_LoadsAutomobileDataFromService()
        {
            // Arrange
            var autos = new List<Automobile>
            {
                new()
                {
                    Year = 2023, Make = "Ford", Model = "Mustang",
                    EngineAnalytics = new EngineAnalytics { Displacement = 350 }
                }
            };
            _automobileDataServiceMock.Setup(s => s.GetAutomobiles()).ReturnsAsync(autos);

            // Act
            var cut = Render<DisplacementResults>();

            // Assert
            cut.WaitForAssertion(() =>
                _automobileDataServiceMock.Verify(s => s.GetAutomobiles(), Times.Once));
        }
    }
}
