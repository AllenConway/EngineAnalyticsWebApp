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

        public async Task<Future> GetFutureWeather(string zipCode)
        {
            try
            {
                var requesturi = $"forecast?zip={zipCode}&units=imperial&cnt=40&appid={apiKey}";
                var results = await http.GetFromJsonAsync<Future>(requesturi);
                if (results?.List != null)
                {
                    results.List = BuildDailyForecasts(results.List);
                }

                return results ?? new Future();
            }
            catch (HttpRequestException ex)
            {
                logger.LogError(ex, "OpenWeatherMap API request failed with status {StatusCode}", ex.StatusCode);
                return new Future();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unexpected error fetching future weather for zip {ZipCode}", zipCode);
                return new Future();
            }
        }

        // Collapses the 3-hour interval forecast into one entry per day. The slot closest
        // to noon represents the day's date/condition, while the high/low are aggregated
        // across every slot for the day (OpenWeather's per-slot temp_min/temp_max are
        // scoped to that slot, so a single slot would report an identical high and low).
        private static ForecastItem[] BuildDailyForecasts(ForecastItem[] forecast)
        {
            return forecast
                .GroupBy(x => DateTime.Parse(x.DtTxt!).Date)
                .Select(g =>
                {
                    var representative = g.OrderBy(x => Math.Abs(DateTime.Parse(x.DtTxt!).Hour - 12)).First();
                    return new ForecastItem
                    {
                        Dt = representative.Dt,
                        DtTxt = representative.DtTxt,
                        Weather = representative.Weather,
                        Wind = representative.Wind,
                        Main = representative.Main == null ? null : new Main
                        {
                            Temp = representative.Main.Temp,
                            FeelsLike = representative.Main.FeelsLike,
                            Pressure = representative.Main.Pressure,
                            Humidity = representative.Main.Humidity,
                            TempMax = g.Max(x => x.Main?.TempMax ?? double.MinValue),
                            TempMin = g.Min(x => x.Main?.TempMin ?? double.MaxValue),
                        }
                    };
                })
                .Take(5)
                .ToArray();
        }

    }
}
