using System.Diagnostics;
using EngineAnalyticsWebApp.Shared.Models.Engine;

namespace EngineAnalyticsWebApp.Components.Calculations.Services
{
    // Single, deterministic CPU-bound calculation shared by BOTH execution modes:
    // - Main thread: called directly from the Blazor component
    // - Web Worker: called from a [JSExport] method running in a separate .NET runtime inside the worker
    public static class EngineSimulationCalculator
    {
        public const int PointsPerSweep = 10_000;
        private const double MinRpm = 1_000;
        private const double MaxRpm = 7_500;
        private const double HorsepowerConstant = 5252;

        public static EngineSimulationResult Run(int sweeps)
        {
            if (sweeps <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(sweeps), "Sweeps must be greater than zero.");
            }

            var stopwatch = Stopwatch.StartNew();

            double peakHorsepower = 0;
            double peakHorsepowerRpm = 0;
            // Accumulating every result prevents the compiler/runtime from eliminating the workload
            double checksum = 0;

            for (int sweep = 0; sweep < sweeps; sweep++)
            {
                // Simulated ambient air density varies slightly per dyno sweep
                double airDensity = 1.0 - 0.0005 * (sweep % 40);

                for (int point = 0; point < PointsPerSweep; point++)
                {
                    double rpm = MinRpm + (MaxRpm - MinRpm) * point / (PointsPerSweep - 1);
                    double torque = SyntheticTorque(rpm, sweep) * airDensity;

                    // Horsepower = Torque x RPM / 5252
                    double horsepower = torque * rpm / HorsepowerConstant;

                    checksum += horsepower;
                    if (horsepower > peakHorsepower)
                    {
                        peakHorsepower = horsepower;
                        peakHorsepowerRpm = rpm;
                    }
                }
            }

            stopwatch.Stop();

            return new EngineSimulationResult
            {
                OperatingPoints = (long)sweeps * PointsPerSweep,
                PeakHorsepower = Math.Round(peakHorsepower, 2),
                PeakHorsepowerRpm = Math.Round(peakHorsepowerRpm),
                Checksum = Math.Round(checksum / ((long)sweeps * PointsPerSweep), 4),
                ElapsedMilliseconds = stopwatch.Elapsed.TotalMilliseconds
            };
        }

        // Synthetic torque curve (lb-ft): bell curve peaking near 4,200 RPM with a small combustion ripple
        private static double SyntheticTorque(double rpm, int sweep)
        {
            double x = (rpm - 4_200) / 2_600;
            double baseTorque = 60 + 420 * Math.Exp(-x * x);
            double ripple = 1.0 + 0.01 * Math.Sin(rpm * 0.01 + sweep);
            return baseTorque * ripple;
        }
    }
}
