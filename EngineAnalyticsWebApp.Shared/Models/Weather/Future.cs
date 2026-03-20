using System.Text.Json.Serialization;

namespace EngineAnalyticsWebApp.Shared.Models.Weather
{
    public class Future
    {
        [JsonPropertyName("cod")]
        public string? Cod { get; set; }

        [JsonPropertyName("cnt")]
        public int Cnt { get; set; }

        [JsonPropertyName("list")]
        public ForecastItem[]? List { get; set; }

        [JsonPropertyName("city")]
        public ForecastCity? City { get; set; }
    }
}
