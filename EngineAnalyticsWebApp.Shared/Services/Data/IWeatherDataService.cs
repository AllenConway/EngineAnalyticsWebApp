using EngineAnalyticsWebApp.Shared.Models.Weather;

namespace EngineAnalyticsWebApp.Shared.Services.Data
{
    public interface IWeatherDataService
    {
        Task<Current> GetCurrentWeather(string zipCode);

        Task<Future> GetFutureWeather(string zipCode);
    }
}
