using EngineAnalyticsWebApp.Components.Calculations.Services;
using EngineAnalyticsWebApp.Shared.Models.Engine;
using EngineAnalyticsWebApp.Shared.Services.Data;

namespace EngineAnalyticsWebApp.Components.Calculations
{
    public partial class HorsepowerCalculation(
        IAutomobileDataService automobileDataService,
        IEngineCalculationsService engineCalculationsService)
    {
        private Automobile automobile = new()
        {
            Horsepower = new Horsepower(),
            EngineAnalytics = new EngineAnalytics() { FlywheelHorsepower = 0, RearWheelHorsepower = 0 }
        };


        private string statusMessage = string.Empty;
        private string alertClass = string.Empty;

        private async Task HandleValidSubmit()
        {
            if (automobile.Horsepower?.Weight != null && automobile.Horsepower?.EstimatedTime != null)
            {
                automobile.EngineAnalytics = engineCalculationsService.CalculateEngineHorsepower(automobile.Horsepower);
                await automobileDataService.AddAutomobile(automobile);
                statusMessage = "Successfully saved calculations";
                alertClass = "alert-success";
            }
            else
            {
                statusMessage = "Required fields are missing";
                alertClass = "alert-danger";
            }
        }

        private void HandleInValidSubmit()
        {
            statusMessage = "Required fields are missing";
            alertClass = "alert-danger";
        }

        private void ClearForm()
        {
            statusMessage = string.Empty;
            alertClass = string.Empty;
        }
    }
}
