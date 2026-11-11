namespace EngineAnalyticsWebApp.Shared.Models.Engine
{
    public class EngineSimulationResult
    {
        public long OperatingPoints { get; set; }
        public double PeakHorsepower { get; set; }
        public double PeakHorsepowerRpm { get; set; }
        public double Checksum { get; set; }
        public double ElapsedMilliseconds { get; set; }
    }
}
