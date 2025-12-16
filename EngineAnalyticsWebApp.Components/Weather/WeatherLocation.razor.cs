using EngineAnalyticsWebApp.Components.Weather.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using EngineAnalyticsWebApp.Shared.Models.Weather;

namespace EngineAnalyticsWebApp.Components.Weather
{
    public partial class WeatherLocation(IWeatherService weatherService, IJSRuntime js)
    {
        private string? zipCode;
        private IJSObjectReference? module;

        protected override async Task OnInitializedAsync()
        {
            Console.WriteLine("1. OnInitializedAsync executed");
            await weatherService.SetWeatherZipCode(zipCode);
        }

        public override async Task SetParametersAsync(ParameterView parameters)
        {
            Console.WriteLine("2. SetParametersAsync executed");
            await base.SetParametersAsync(parameters);
        }

        protected override async Task OnParametersSetAsync()
        {
            Console.WriteLine("3. OnParametersSetAsync executed");
            await base.OnParametersSetAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            Console.WriteLine("4. OnAfterRenderAsync executed");
            if (firstRender)
            {
                module = await js.InvokeAsync<IJSObjectReference>(
                    "import", "./_content/EngineAnalyticsWebApp.Components/Weather/WeatherLocation.razor.js");
            }
        }

        public async Task UpdateZipCode(KeyboardEventArgs e)
        {
            if ((e.Code == "Enter" || e.Code == "NumpadEnter") && !string.IsNullOrEmpty(zipCode))
            {
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
