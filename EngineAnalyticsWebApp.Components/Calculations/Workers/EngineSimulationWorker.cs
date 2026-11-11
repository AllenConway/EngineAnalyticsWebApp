using System.Runtime.InteropServices.JavaScript;
using System.Runtime.Versioning;
using System.Text.Json;
using EngineAnalyticsWebApp.Components.Calculations.Services;

namespace EngineAnalyticsWebApp.Components.Calculations.Workers
{
    // Exposed to JavaScript via [JSExport] and invoked by wwwroot/engineSimulationWorker.js
    // inside a dedicated Web Worker that hosts its own .NET WebAssembly runtime.
    // [JSExport] supports interop-friendly types only, so the result is returned as JSON.
    [SupportedOSPlatform("browser")]
    public static partial class EngineSimulationWorker
    {
        [JSExport]
        public static string RunSimulation(int sweeps)
        {
            var result = EngineSimulationCalculator.Run(sweeps);
            return JsonSerializer.Serialize(result);
        }
    }
}
