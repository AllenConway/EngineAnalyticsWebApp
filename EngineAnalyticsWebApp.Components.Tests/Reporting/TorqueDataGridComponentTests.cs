using Bunit;
using EngineAnalyticsWebApp.Components.Reporting;
using EngineAnalyticsWebApp.Shared.Models.Engine;
using EngineAnalyticsWebApp.Shared.Services.Data;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace EngineAnalyticsWebApp.Components.Tests.Reporting
{
    public class TorqueDataGridComponentTests : BlazoriseComponentTestContext
    {
        private readonly Mock<IAutomobileDataService> _automobileDataServiceMock;

        public TorqueDataGridComponentTests()
        {
            _automobileDataServiceMock = new Mock<IAutomobileDataService>();
            Services.AddSingleton(_automobileDataServiceMock.Object);
        }

        [Fact]
        public void TorqueDataGrid_WithData_RendersTorqueValues()
        {
            // Arrange
            var autos = new List<Automobile>
            {
                new() { Year = 2023, Make = "Ford", Model = "Mustang",
                    EngineAnalytics = new EngineAnalytics { Torque = 420 } }
            };
            _automobileDataServiceMock.Setup(s => s.GetAutomobiles()).ReturnsAsync(autos);

            // Act
            var cut = Render<TorqueDataGrid>();

            // Assert
            cut.WaitForAssertion(() => Assert.Contains("Mustang", cut.Markup));
        }

        [Fact]
        public void TorqueDataGrid_FiltersOutZeroTorqueEntries()
        {
            // Arrange
            var autos = new List<Automobile>
            {
                new() { Year = 2023, Make = "Ford", Model = "Mustang",
                    EngineAnalytics = new EngineAnalytics { Torque = 420 } },
                new() { Year = 2000, Make = "Honda", Model = "Civic",
                    EngineAnalytics = new EngineAnalytics { Torque = 0 } }
            };
            _automobileDataServiceMock.Setup(s => s.GetAutomobiles()).ReturnsAsync(autos);

            // Act
            var cut = Render<TorqueDataGrid>();

            // Assert
            cut.WaitForAssertion(() => Assert.Contains("Mustang", cut.Markup));
            Assert.DoesNotContain("Civic", cut.Markup);
        }

        [Fact]
        public void TorqueDataGrid_LoadsDataFromService()
        {
            // Arrange
            _automobileDataServiceMock.Setup(s => s.GetAutomobiles()).ReturnsAsync(new List<Automobile>());

            // Act
            var cut = Render<TorqueDataGrid>();

            // Assert
            cut.WaitForAssertion(() =>
                _automobileDataServiceMock.Verify(s => s.GetAutomobiles(), Times.Once));
        }
    }
}
