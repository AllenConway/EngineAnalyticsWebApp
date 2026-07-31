using EngineAnalyticsWebApp.Components.Weather.Services;
using EngineAnalyticsWebApp.Shared.Models.Weather;
using EngineAnalyticsWebApp.Shared.Services.Data;
using Microsoft.AspNetCore.Components;
using System.Reactive.Linq;

namespace EngineAnalyticsWebApp.Components.Weather
{
    public partial class WeatherFuture
    {
        private readonly IWeatherService weatherService;
        private readonly IWeatherDataService weatherDataService;
        private Future futureWeatherData = new();
        private IDisposable? subscription;

        public WeatherFuture(IWeatherService weatherService, IWeatherDataService weatherDataService)
        {
            this.weatherService = weatherService;
            this.weatherDataService = weatherDataService;
        }

        protected override void OnInitialized()
        {
            subscription = weatherService.GetCurrentZipCodeStream()
                .Subscribe(async data => await OnZipCodeDataLoaded(data));
        }

        private async Task OnZipCodeDataLoaded(string zipCode)
        {
            futureWeatherData = await weatherDataService.GetFutureWeather(zipCode);
            // StateHasChanged is required here because the UI is being updated from
            // an observable subscription, which runs outside Blazor's normal rendering pipeline,
            // and the async nature post-await would not update the UI until next action otherwise
            StateHasChanged();
        }

        public void Dispose()
        {
            subscription?.Dispose();
        }
    }
}
