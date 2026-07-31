using System.Text.Json;
using System.Text.Json.Serialization;

namespace EngineAnalyticsWebApp.Shared.Models.Weather.Json
{
    /// <summary>
    /// Rounds temperature values to whole numbers as they are read from the API,
    /// so the domain model exposes forecast-friendly values (e.g. 62 instead of 61.88).
    /// </summary>
    public class RoundedDoubleConverter : JsonConverter<double>
    {
        public override double Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return Math.Round(reader.GetDouble(), MidpointRounding.AwayFromZero);
        }

        public override void Write(Utf8JsonWriter writer, double value, JsonSerializerOptions options)
        {
            writer.WriteNumberValue(Math.Round(value, MidpointRounding.AwayFromZero));
        }
    }
}
