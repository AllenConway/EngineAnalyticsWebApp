using System.Text.Json.Serialization;
using EngineAnalyticsWebApp.Shared.Models.Weather.Json;

namespace EngineAnalyticsWebApp.Shared.Models.Weather
{
    public class Main
    {
        [JsonPropertyName("temp")]
        [JsonConverter(typeof(RoundedDoubleConverter))]
        public double Temp { get; set; }

        [JsonPropertyName("feels_like")]
        [JsonConverter(typeof(RoundedDoubleConverter))]
        public double FeelsLike { get; set; }

        [JsonPropertyName("temp_min")]
        [JsonConverter(typeof(RoundedDoubleConverter))]
        public double TempMin { get; set; }

        [JsonPropertyName("temp_max")]
        [JsonConverter(typeof(RoundedDoubleConverter))]
        public double TempMax { get; set; }

        [JsonPropertyName("pressure")]
        public double Pressure { get; set; }

        [JsonPropertyName("humidity")]
        public double Humidity { get; set; }
    }
}
