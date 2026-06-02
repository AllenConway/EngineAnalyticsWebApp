using Bunit;
using EngineAnalyticsWebApp.Shared.Models.Engine;
using EngineAnalyticsWebApp.Shared.Services.Data;
using EngineAnalyticsWebApp.UI.Components.Pages.Reports;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace EngineAnalyticsWebApp.UI.Tests.Components.Pages.Reports
{
    public class TorqueResultsTests : BlazoriseTestContext
    {
        private readonly Mock<IAutomobileDataService> _automobileDataServiceMock;

        public TorqueResultsTests()
        {
            _automobileDataServiceMock = new Mock<IAutomobileDataService>();
            Services.AddSingleton(_automobileDataServiceMock.Object);
        }

        [Fact]
        public void TorqueResults_RendersReportHeaderTitle()
        {
            // Arrange
            _automobileDataServiceMock.Setup(s => s.GetAutomobiles()).ReturnsAsync(new List<Automobile>());

            // Act
            var cut = Render<TorqueResults>();

            // Assert
            Assert.Contains("Torque Results", cut.Markup);
        }

        [Fact]
        public void TorqueResults_RendersDescription()
        {
            // Arrange
            _automobileDataServiceMock.Setup(s => s.GetAutomobiles()).ReturnsAsync(new List<Automobile>());

            // Act
            var cut = Render<TorqueResults>();

            // Assert
            Assert.Contains("logged engine torque calculation results", cut.Markup);
        }

        [Fact]
        public void TorqueResults_LoadsAutomobileDataFromService()
        {
            // Arrange
            var autos = new List<Automobile>
            {
                new()
                {
                    Year = 2023, Make = "Ford", Model = "Mustang",
                    EngineAnalytics = new EngineAnalytics { Torque = 420 }
                }
            };
            _automobileDataServiceMock.Setup(s => s.GetAutomobiles()).ReturnsAsync(autos);

            // Act
            var cut = Render<TorqueResults>();

            // Assert
            cut.WaitForAssertion(() =>
                _automobileDataServiceMock.Verify(s => s.GetAutomobiles(), Times.Once));
        }
    }
}
