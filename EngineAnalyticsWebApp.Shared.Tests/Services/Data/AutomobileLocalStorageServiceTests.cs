using Blazored.LocalStorage;
using EngineAnalyticsWebApp.Shared.Models.Engine;
using EngineAnalyticsWebApp.Shared.Services.Data;
using Moq;

namespace EngineAnalyticsWebApp.Shared.Tests.Services.Data
{
    public class AutomobileLocalStorageServiceTests
    {
        private readonly Mock<ILocalStorageService> _localStorageMock;
        private readonly AutomobileLocalStorageService _sut;

        public AutomobileLocalStorageServiceTests()
        {
            _localStorageMock = new Mock<ILocalStorageService>();
            _sut = new AutomobileLocalStorageService(_localStorageMock.Object);
        }

        [Fact]
        public async Task GetAutomobiles_WhenNoDataExists_ReturnsEmptyCollection()
        {
            // Arrange
            _localStorageMock
                .Setup(s => s.GetItemAsync<IEnumerable<Automobile>>(It.IsAny<string>(), default))
                .ReturnsAsync((IEnumerable<Automobile>?)null);

            // Act
            var result = await _sut.GetAutomobiles();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAutomobiles_WhenDataExists_ReturnsAutomobiles()
        {
            // Arrange
            var automobiles = new List<Automobile>
            {
                new() { Year = 2023, Make = "Ford", Model = "Mustang" }
            };
            _localStorageMock
                .Setup(s => s.GetItemAsync<IEnumerable<Automobile>>(It.IsAny<string>(), default))
                .ReturnsAsync(automobiles);

            // Act
            var result = await _sut.GetAutomobiles();

            // Assert
            Assert.Single(result);
        }

        [Fact]
        public async Task AddAutomobile_WithValidAutomobile_SavesData()
        {
            // Arrange
            var automobile = new Automobile { Year = 2023, Make = "Ford", Model = "Mustang" };
            _localStorageMock
                .Setup(s => s.GetItemAsync<IEnumerable<Automobile>>(It.IsAny<string>(), default))
                .ReturnsAsync(new List<Automobile>());

            // Act
            await _sut.AddAutomobile(automobile);

            // Assert
            _localStorageMock.Verify(s => s.SetItemAsync(
                It.IsAny<string>(),
                It.Is<IEnumerable<Automobile>>(a => a.Count() == 1),
                default), Times.Once);
        }

        [Fact]
        public async Task AddAutomobile_WithNullAutomobile_DoesNotSaveData()
        {
            // Arrange
            // Act
            await _sut.AddAutomobile(null!);

            // Assert
            _localStorageMock.Verify(s => s.SetItemAsync(
                It.IsAny<string>(),
                It.IsAny<IEnumerable<Automobile>>(),
                default), Times.Never);
        }

        [Fact]
        public async Task AddAutomobile_AppendsToExistingCollection()
        {
            // Arrange
            var existing = new List<Automobile>
            {
                new() { Year = 2022, Make = "Chevy", Model = "Camaro" }
            };
            _localStorageMock
                .Setup(s => s.GetItemAsync<IEnumerable<Automobile>>(It.IsAny<string>(), default))
                .ReturnsAsync(existing);

            var newAuto = new Automobile { Year = 2023, Make = "Ford", Model = "Mustang" };

            // Act
            await _sut.AddAutomobile(newAuto);

            // Assert
            _localStorageMock.Verify(s => s.SetItemAsync(
                It.IsAny<string>(),
                It.Is<IEnumerable<Automobile>>(a => a.Count() == 2),
                default), Times.Once);
        }
    }
}
