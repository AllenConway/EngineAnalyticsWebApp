using Bunit;
using EngineAnalyticsWebApp.Shared.Models.Engine;
using EngineAnalyticsWebApp.Shared.Services.Data;
using EngineAnalyticsWebApp.UI.Components.Pages.Reports;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace EngineAnalyticsWebApp.UI.Tests.Components.Pages.Reports
{
    public class HorsepowerResultsTests : BunitContext
    {
        private readonly Mock<IAutomobileDataService> _automobileDataServiceMock;

        public HorsepowerResultsTests()
        {
            _automobileDataServiceMock = new Mock<IAutomobileDataService>();
            Services.AddSingleton(_automobileDataServiceMock.Object);
        }

        [Fact]
        public void HorsepowerResults_WithData_RendersTableRows()
        {
            // Arrange
            var autos = new List<Automobile>
            {
                new()
                {
                    Year = 2023, Make = "Ford", Model = "Mustang",
                    EngineAnalytics = new EngineAnalytics { RearWheelHorsepower = 354, FlywheelHorsepower = 406 }
                }
            };
            _automobileDataServiceMock.Setup(s => s.GetAutomobiles()).ReturnsAsync(autos);

            // Act
            var cut = Render<HorsepowerResults>();

            // Assert
            cut.WaitForAssertion(() => Assert.Contains("Mustang", cut.Markup));
            Assert.Contains("354", cut.Markup);
            Assert.Contains("406", cut.Markup);
        }

        [Fact]
        public void HorsepowerResults_RendersReportHeaderTitle()
        {
            // Arrange
            _automobileDataServiceMock.Setup(s => s.GetAutomobiles()).ReturnsAsync(new List<Automobile>());

            // Act
            var cut = Render<HorsepowerResults>();

            // Assert
            Assert.Contains("Horsepower Results", cut.Markup);
        }

        [Fact]
        public void HorsepowerResults_WithNoData_RendersHeaderColumns()
        {
            // Arrange
            _automobileDataServiceMock.Setup(s => s.GetAutomobiles()).ReturnsAsync(new List<Automobile>());

            // Act
            var cut = Render<HorsepowerResults>();

            // Assert
            cut.WaitForAssertion(() => Assert.Contains("Rear Wheel Horsepower", cut.Markup));
            Assert.Contains("Flywheel Horsepower", cut.Markup);
        }

        [Fact]
        public void HorsepowerResults_FiltersOutZeroHorsepowerEntries()
        {
            // Arrange
            var autos = new List<Automobile>
            {
                new()
                {
                    Year = 2023, Make = "Ford", Model = "Mustang",
                    EngineAnalytics = new EngineAnalytics { RearWheelHorsepower = 354 }
                },
                new()
                {
                    Year = 2000, Make = "Honda", Model = "Civic",
                    EngineAnalytics = new EngineAnalytics { RearWheelHorsepower = 0 }
                }
            };
            _automobileDataServiceMock.Setup(s => s.GetAutomobiles()).ReturnsAsync(autos);

            // Act
            var cut = Render<HorsepowerResults>();

            // Assert
            cut.WaitForAssertion(() => Assert.Contains("Mustang", cut.Markup));
            Assert.DoesNotContain("Civic", cut.Markup);
        }
    }
}
