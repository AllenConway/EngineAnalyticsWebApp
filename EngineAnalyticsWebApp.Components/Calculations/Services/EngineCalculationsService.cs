using EngineAnalyticsWebApp.Shared.Models.Engine;

namespace EngineAnalyticsWebApp.Components.Calculations.Services
{
    public class EngineCalculationsService : IEngineCalculationsService
    {
        public EngineAnalytics CalculateEngineHorsepower(Horsepower horsepower)
        {
            var rwHorsepower = 0.0;

            var hpCalc = horsepower.EstimatedTime / 5.825;
            if (hpCalc.HasValue && horsepower.Weight.HasValue)
            {
                double hp = hpCalc.Value;
                double weight = horsepower.Weight.Value;
                rwHorsepower = Math.Round(weight / Math.Pow(hp, 3));
            }

            //15 percent drivetrain loss on wheel and increase at flywheel (engine horsepower)
            var flywheelHP = (rwHorsepower * 1.146);
            var fwHorsepower = Math.Round(flywheelHP);

            EngineAnalytics hpCalcs = new EngineAnalytics
            {
                RearWheelHorsepower = rwHorsepower,
                FlywheelHorsepower = fwHorsepower
            };

            return hpCalcs;
        }

        public EngineAnalytics CalculateEngineDisplacement(Displacement displacement)
        {
            var displacementValue = 0.0;

            if (displacement.BoreSize.HasValue && displacement.CrankshaftStrokeLength.HasValue && displacement.Cylinders.HasValue)
            {
                var radius = (displacement.BoreSize.Value / 2);
                displacementValue = Math.Round(Math.Pow(radius, 2) * Math.PI * displacement.CrankshaftStrokeLength.Value * displacement.Cylinders.Value);
            }

            EngineAnalytics displacementCalcs = new EngineAnalytics
            {
                Displacement = displacementValue
            };

            return displacementCalcs;
        }

        public EngineAnalytics CalculateEngineTorque(Torque torque)
        {
            var torqueValue = 0.0;

            if (torque.Horsepower.HasValue && torque.EngineRPM.HasValue)
            {
                torqueValue = Math.Round((torque.Horsepower.Value * 5252) / torque.EngineRPM.Value);
            }

            EngineAnalytics torqueCalcs = new EngineAnalytics
            {
                Torque = torqueValue
            };

            return torqueCalcs;
        }
    }
}
