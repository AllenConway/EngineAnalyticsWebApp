using Bunit;
using EngineAnalyticsWebApp.Components.Reporting;
using EngineAnalyticsWebApp.Shared.Models.Engine;
using EngineAnalyticsWebApp.Shared.Services.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace EngineAnalyticsWebApp.Components.Tests.Reporting
{
    public class HorsepowerDataGridComponentTests : BunitContext
    {
        private readonly Mock<IAutomobileDataService> _automobileDataServiceMock;

        public HorsepowerDataGridComponentTests()
        {
            _automobileDataServiceMock = new Mock<IAutomobileDataService>();
            Services.AddSingleton(_automobileDataServiceMock.Object);
        }

        private static RenderFragment TableHeader() => builder =>
        {
            builder.AddMarkupContent(0, "<th>Model</th>");
        };

        private static RenderFragment<Automobile> RowTemplate() => auto => builder =>
        {
            builder.AddMarkupContent(0, $"<tr><td>{auto.Model}</td></tr>");
        };

        [Fact]
        public void HorsepowerDataGrid_WithData_RendersRows()
        {
            // Arrange
            var autos = new List<Automobile>
            {
                new() { Year = 2023, Make = "Ford", Model = "Mustang",
                    EngineAnalytics = new EngineAnalytics { RearWheelHorsepower = 354 } }
            };
            _automobileDataServiceMock.Setup(s => s.GetAutomobiles()).ReturnsAsync(autos);

            // Act
            var cut = Render<HorsepowerDataGrid>(parameters => parameters
                .Add(p => p.TableHeader, TableHeader())
                .Add(p => p.RowTemplate, RowTemplate()));

            // Assert
            cut.WaitForAssertion(() => Assert.Contains("Mustang", cut.Markup));
        }

        [Fact]
        public void HorsepowerDataGrid_FiltersOutZeroHorsepowerEntries()
        {
            // Arrange
            var autos = new List<Automobile>
            {
                new() { Year = 2023, Make = "Ford", Model = "Mustang",
                    EngineAnalytics = new EngineAnalytics { RearWheelHorsepower = 354 } },
                new() { Year = 2000, Make = "Honda", Model = "Civic",
                    EngineAnalytics = new EngineAnalytics { RearWheelHorsepower = 0 } }
            };
            _automobileDataServiceMock.Setup(s => s.GetAutomobiles()).ReturnsAsync(autos);

            // Act
            var cut = Render<HorsepowerDataGrid>(parameters => parameters
                .Add(p => p.TableHeader, TableHeader())
                .Add(p => p.RowTemplate, RowTemplate()));

            // Assert
            cut.WaitForAssertion(() => Assert.Contains("Mustang", cut.Markup));
            Assert.DoesNotContain("Civic", cut.Markup);
        }

        [Fact]
        public void HorsepowerDataGrid_RendersFilterComponent()
        {
            // Arrange
            _automobileDataServiceMock.Setup(s => s.GetAutomobiles()).ReturnsAsync(new List<Automobile>());

            // Act
            var cut = Render<HorsepowerDataGrid>(parameters => parameters
                .Add(p => p.TableHeader, TableHeader())
                .Add(p => p.RowTemplate, RowTemplate()));

            // Assert
            cut.WaitForAssertion(() => Assert.NotNull(cut.Find("#filterInput")));
        }

        [Fact]
        public void HorsepowerDataGrid_WhenFilterApplied_FiltersRowsByModel()
        {
            // Arrange
            var autos = new List<Automobile>
            {
                new() { Year = 2023, Make = "Ford", Model = "Mustang",
                    EngineAnalytics = new EngineAnalytics { RearWheelHorsepower = 354 } },
                new() { Year = 2000, Make = "Honda", Model = "Civic",
                    EngineAnalytics = new EngineAnalytics { RearWheelHorsepower = 200 } }
            };
            _automobileDataServiceMock.Setup(s => s.GetAutomobiles()).ReturnsAsync(autos);

            var cut = Render<HorsepowerDataGrid>(parameters => parameters
                .Add(p => p.TableHeader, TableHeader())
                .Add(p => p.RowTemplate, RowTemplate()));

            // Act
            cut.Find("#filterInput").Change("Mustang");
            cut.Find("button.btn-primary").Click();

            // Assert
            cut.WaitForAssertion(() => Assert.Contains("Mustang", cut.Markup));
            Assert.DoesNotContain("Civic", cut.Markup);
        }

        [Fact]
        public void HorsepowerDataGrid_WhenFilterCleared_ShowsAllRows()
        {
            // Arrange
            var autos = new List<Automobile>
            {
                new() { Year = 2023, Make = "Ford", Model = "Mustang",
                    EngineAnalytics = new EngineAnalytics { RearWheelHorsepower = 354 } },
                new() { Year = 2000, Make = "Honda", Model = "Civic",
                    EngineAnalytics = new EngineAnalytics { RearWheelHorsepower = 200 } }
            };
            _automobileDataServiceMock.Setup(s => s.GetAutomobiles()).ReturnsAsync(autos);

            var cut = Render<HorsepowerDataGrid>(parameters => parameters
                .Add(p => p.TableHeader, TableHeader())
                .Add(p => p.RowTemplate, RowTemplate()));

            cut.Find("#filterInput").Change("Mustang");
            cut.Find("button.btn-primary").Click();

            // Act
            cut.Find("button.btn-secondary").Click();

            // Assert
            cut.WaitForAssertion(() => Assert.Contains("Mustang", cut.Markup));
            Assert.Contains("Civic", cut.Markup);
        }
    }
}
