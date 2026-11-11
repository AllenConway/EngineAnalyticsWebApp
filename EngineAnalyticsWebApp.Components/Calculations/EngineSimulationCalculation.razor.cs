using System.Text.Json;
using EngineAnalyticsWebApp.Components.Calculations.Services;
using EngineAnalyticsWebApp.Shared.Models.Engine;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace EngineAnalyticsWebApp.Components.Calculations
{
    public partial class EngineSimulationCalculation(IJSRuntime js) : IAsyncDisposable
    {
        private const int MaxSweeps = 100_000;

        private readonly (string Name, int Sweeps)[] presets =
        [
            ("Light", 500),
            ("Standard", 2_000),
            ("Heavy", 5_000)
        ];

        private IJSObjectReference? module;
        private int sweeps = 2_000;
        private int clickCount;
        private bool isRunning;
        private bool workerReady;
        private string workerStatus = string.Empty;
        private string? executionMode;
        private EngineSimulationResult? result;
        private string statusMessage = "Idle";
        private string alertClass = "alert-secondary";
        private bool disposed;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                // Warm up the worker (boot its .NET runtime) up front so startup cost isn't measured as calculation time
                workerStatus = "Starting .NET runtime in Web Worker...";
                StateHasChanged();
                try
                {
                    var importedModule = await js.InvokeAsync<IJSObjectReference>(
                        "import", "./_content/EngineAnalyticsWebApp.Components/Calculations/EngineSimulationCalculation.razor.js");
                    if (disposed)
                    {
                        await importedModule.DisposeAsync();
                        return;
                    }

                    module = importedModule;
                    await importedModule.InvokeVoidAsync("initializeWorker");
                    if (disposed)
                    {
                        return;
                    }

                    workerReady = true;
                    workerStatus = "Web Worker ready";
                }
                catch (JSException ex)
                {
                    if (disposed)
                    {
                        return;
                    }

                    workerStatus = "Web Worker unavailable";
                    SetStatus($"Web Worker initialization failed: {ex.Message}", "alert-danger");
                }
                StateHasChanged();
            }
        }

        private void SelectPreset(ChangeEventArgs e)
        {
            if (int.TryParse(e.Value?.ToString(), out var value))
            {
                sweeps = value;
            }
        }

        private void IncrementClickCount() => clickCount++;

        private async Task RunOnMainThread()
        {
            if (!TryBeginRun("Main Thread"))
            {
                return;
            }

            // Let the browser paint the "Running" status before the UI thread is blocked.
            // This delay is NOT part of the measured calculation time.
            StateHasChanged();
            await Task.Delay(1);

            try
            {
                // Synchronous CPU-bound work on the browser UI thread: blocks all UI event handling until complete
                result = EngineSimulationCalculator.Run(sweeps);
                SetStatus("Simulation completed on the main thread", "alert-success");
            }
            catch (Exception ex)
            {
                SetStatus($"Simulation failed: {ex.Message}", "alert-danger");
            }
            finally
            {
                isRunning = false;
            }
        }

        private async Task RunInWebWorker()
        {
            if (module is null || !workerReady || !TryBeginRun("Web Worker"))
            {
                return;
            }

            try
            {
                var json = await module.InvokeAsync<string>("runSimulation", sweeps);
                result = JsonSerializer.Deserialize<EngineSimulationResult>(json);
                SetStatus("Simulation completed in the Web Worker", "alert-success");
            }
            catch (JSException ex)
            {
                SetStatus($"Web Worker simulation failed: {ex.Message}", "alert-danger");
            }
            finally
            {
                isRunning = false;
            }
        }

        private bool TryBeginRun(string mode)
        {
            if (isRunning)
            {
                return false;
            }

            if (sweeps < 1 || sweeps > MaxSweeps)
            {
                SetStatus($"Dyno sweeps must be between 1 and {MaxSweeps:N0}", "alert-danger");
                return false;
            }

            isRunning = true;
            executionMode = mode;
            result = null;
            SetStatus($"Running simulation on the {mode}...", "alert-info");
            return true;
        }

        private void SetStatus(string message, string cssClass)
        {
            statusMessage = message;
            alertClass = cssClass;
        }

        public async ValueTask DisposeAsync()
        {
            disposed = true;

            if (module is not null)
            {
                try
                {
                    await module.InvokeVoidAsync("disposeWorker");
                    await module.DisposeAsync();
                }
                catch (JSDisconnectedException)
                {
                }
            }
        }
    }
}
