using System.Text.Json.Serialization;

namespace EngineAnalyticsWebApp.Shared.Models.Weather
{
    public class ForecastCity
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("country")]
        public string? Country { get; set; }

        [JsonPropertyName("coord")]
        public Coord? Coord { get; set; }

        [JsonPropertyName("timezone")]
        public int Timezone { get; set; }
    }
}
