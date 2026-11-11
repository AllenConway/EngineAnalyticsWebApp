// Runs inside a dedicated Web Worker (module worker).
// Boots a separate .NET WebAssembly runtime and invokes the [JSExport] C# method
// EngineAnalyticsWebApp.Components.Calculations.Workers.EngineSimulationWorker.RunSimulation.
// No simulation math lives in JavaScript; this file only handles bootstrapping and messaging.
import { dotnet } from '../../_framework/dotnet.js';

let simulationWorker;
let startupError;

try {
    const { getAssemblyExports } = await dotnet.create();
    const exports = await getAssemblyExports('EngineAnalyticsWebApp.Components');
    simulationWorker = exports.EngineAnalyticsWebApp.Components.Calculations.Workers.EngineSimulationWorker;
    self.postMessage({ command: 'ready' });
} catch (err) {
    startupError = err?.message ?? String(err);
    self.postMessage({ command: 'startupError', error: startupError });
}

self.addEventListener('message', e => {
    const { command, requestId } = e.data;
    try {
        if (!simulationWorker) {
            throw new Error(startupError || 'Worker .NET exports not loaded');
        }

        if (command !== 'runSimulation') {
            throw new Error(`Unknown command: ${command}`);
        }

        const result = simulationWorker.RunSimulation(e.data.sweeps);
        self.postMessage({ command: 'response', requestId, result });
    } catch (err) {
        self.postMessage({ command: 'response', requestId, error: err?.message ?? String(err) });
    }
});
