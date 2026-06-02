using Bunit;
using Blazorise;
using Blazorise.Bootstrap;
using Blazorise.Icons.FontAwesome;
using Microsoft.Extensions.DependencyInjection;

namespace EngineAnalyticsWebApp.Components.Tests
{
    /// <summary>
    /// Base bUnit test context that registers the Blazorise services required by
    /// components which rely on Blazorise UI elements (Table, DataGrid, etc.).
    /// JSInterop is placed in loose mode so Blazorise's JS calls do not fail the render.
    ///
    /// Implements <see cref="IAsyncLifetime"/> so xUnit disposes the context asynchronously.
    /// This is required because Blazorise registers JS modules that only support
    /// <see cref="IAsyncDisposable"/> and would throw during synchronous disposal.
    /// </summary>
    public abstract class BlazoriseComponentTestContext : BunitContext, IAsyncLifetime
    {
        protected BlazoriseComponentTestContext()
        {
            // Arrange: Blazorise components invoke JS during render; loose mode returns defaults.
            JSInterop.Mode = JSRuntimeMode.Loose;

            Services
                .AddBlazorise(options => options.Immediate = true)
                .AddBootstrapProviders()
                .AddFontAwesomeIcons();
        }

        public Task InitializeAsync() => Task.CompletedTask;

        async Task IAsyncLifetime.DisposeAsync()
        {
            // Dispose the bUnit context asynchronously to satisfy Blazorise's async-only modules.
            await ((IAsyncDisposable)this).DisposeAsync();
        }
    }
}
