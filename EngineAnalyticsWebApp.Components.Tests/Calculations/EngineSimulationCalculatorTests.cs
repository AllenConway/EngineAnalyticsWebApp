using EngineAnalyticsWebApp.Components.Calculations.Services;

namespace EngineAnalyticsWebApp.Components.Tests.Calculations
{
    public class EngineSimulationCalculatorTests
    {
        [Fact]
        public void Run_ChecksumIsAverageHorsepowerAcrossOperatingPoints()
        {
            var result = EngineSimulationCalculator.Run(1);

            Assert.Equal(EngineSimulationCalculator.PointsPerSweep, result.OperatingPoints);
            Assert.InRange(result.Checksum, 0, result.PeakHorsepower);
        }
    }
}
