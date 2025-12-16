using EngineAnalyticsWebApp.Components.Weather.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace EngineAnalyticsWebApp.UI.Components.Pages.Weather
{
    public partial class TrackWeather
    {
        private string title = "Track Weather";
        private string? zipCode = "";

        public TrackWeather(IWeatherService weatherService)
        {
            this.weatherService = weatherService;
        }

        private IWeatherService weatherService { get; }

        protected override async Task OnInitializedAsync()
        {
            await weatherService.SetWeatherZipCode(zipCode);
        }

        public async Task UpdateZipCode(KeyboardEventArgs e)
        {
            if ((e.Code == "Enter" || e.Code == "NumpadEnter") && !string.IsNullOrEmpty(zipCode))
            {
                // Call service to set zip code
                await weatherService.SetWeatherZipCode(zipCode);
            }         
        }
    }
}
