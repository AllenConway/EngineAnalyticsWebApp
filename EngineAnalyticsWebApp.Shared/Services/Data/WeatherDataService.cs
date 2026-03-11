using EngineAnalyticsWebApp.Shared.Models.Weather;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace EngineAnalyticsWebApp.Shared.Services.Data
{
    public class WeatherDataService(HttpClient http, IConfiguration configuration, ILogger<WeatherDataService> logger) : IWeatherDataService
    {
        private readonly string apiKey = configuration["OpenWeatherMap:ApiKey"]
            ?? throw new InvalidOperationException("OpenWeatherMap:ApiKey is not configured. See README for setup instructions.");

        public async Task<Current> GetCurrentWeather(string zipCode)
        {
            try
            {
                // Build out query string parameters for Open Weather API
                var requesturi = $"weather?zip={zipCode}&units=imperial&appid={apiKey}";

                var results = await http.GetFromJsonAsync<Current>(requesturi);
                return results ?? new Current();
            }
            catch (HttpRequestException ex)
            {
                logger.LogError(ex, "OpenWeatherMap API request failed with status {StatusCode}", ex.StatusCode);
                return new Current();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unexpected error fetching current weather for zip {ZipCode}", zipCode);
                return new Current();
            }
        }

        public Task<IEnumerable<Future>> GetFutureWeather()
        {
            throw new NotImplementedException();
        }

    }
}
