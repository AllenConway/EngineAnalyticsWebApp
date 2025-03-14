using EngineAnalyticsWebApp.Components.Weather.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using EngineAnalyticsWebApp.Shared.Models.Weather;

namespace EngineAnalyticsWebApp.Components.Weather
{
    public partial class WeatherLocation
    {
        private string? zipCode;
        private IJSObjectReference? module;

        [Inject]
        private IWeatherService weatherService { get; set; } = default!;
        [Inject]
        private IJSRuntime JS { get; set; } = default!;

        /// <summary>
        /// Called when the component is initialized. This is where you can perform any setup work
        /// that requires asynchronous operations, such as fetching data from a service.
        /// </summary>
        /// <returns></returns>
        protected override async Task OnInitializedAsync()
        {
            Console.WriteLine("1. OnInitializedAsync executed");
            await weatherService.SetWeatherZipCode(zipCode);
        }

        /// <summary>
        /// Called when the component receives parameters from its parent component.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public override async Task SetParametersAsync(ParameterView parameters)
        {
            Console.WriteLine("2. SetParametersAsync executed");
            await base.SetParametersAsync(parameters);
        }

        /// <summary>
        /// Called when the component's parameters have been set or updated. This is 
        /// useful for handling parameter changes that require asynchronous operations.
        /// </summary>
        /// <returns></returns>
        protected override async Task OnParametersSetAsync()
        {
            Console.WriteLine("3. OnParametersSetAsync executed");
            await base.OnParametersSetAsync();
        }

        /// <summary>
        /// Called after the component has been rendered. This is useful for performing post-rendering
        /// operations that require asynchronous work, such as interacting with JavaScript.
        /// </summary>
        /// <param name="firstRender"></param>
        /// <returns></returns>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            Console.WriteLine("4. OnAfterRenderAsync executed");
            if (firstRender)
            {
                // Import JS module ansyncronously as this requires a network request
                // Note prior to this call, you will not see this script as a resource in the browser
                module = await JS.InvokeAsync<IJSObjectReference>(
                    "import", "./_content/EngineAnalyticsWebApp.Components/Weather/WeatherLocation.razor.js");
                    // "import", "./_content/EngineAnalyticsWebApp.Components/bundle.min.js");
            }

            // await Alert("The bundle file indeed works, and JS interop is still intact.");
        }

        public async Task UpdateZipCode(KeyboardEventArgs e)
        {
            if ((e.Code == "Enter" || e.Code == "NumpadEnter") && !string.IsNullOrEmpty(zipCode))
            {
                // Call service to set zip code
                await weatherService.SetWeatherZipCode(zipCode);
            }
        }

        public async Task useCurrentGeolocation()
        {
            var coords = await UseCurrentGeolocation();
            if(coords.Latitude is not null)
            {
                await Alert($"Your location is Latitude: {coords.Latitude} and Longitude: {coords.Longitude}");
            }
            else
            {
                await Alert("Cannot retrieve location information");
            }
        }

        public async ValueTask<string?> Prompt(string message)
        {
            return module is not null ?
                await module.InvokeAsync<string>("showPrompt", message) : null;
        }

        public async Task Alert(string message)
        {
            // Use InvokeVoidAsync for calls not returning a value
            if (module is not null)
                await module.InvokeVoidAsync("showAlert", message);
        }

        public async ValueTask<Geolocation> UseCurrentGeolocation()
        {
            return module is not null ? 
                await module.InvokeAsync<Geolocation>("useCurrentGeolocationAsync") : new Geolocation();
        }

        public async ValueTask DisposeAsync()
        {
            if (module is not null)
                await module.DisposeAsync();
        }
    }

}
