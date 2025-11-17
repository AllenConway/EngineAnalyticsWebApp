using EngineAnalyticsWebApp.Components.Calculations.Services;
using EngineAnalyticsWebApp.Shared.Models.Engine;
using EngineAnalyticsWebApp.Shared.Services.Data;

namespace EngineAnalyticsWebApp.Components.Calculations
{
    public partial class DisplacementCalculation(
        IAutomobileDataService automobileDataService,
        IEngineCalculationsService engineCalculationsService)
    {

        private Automobile automobile = new()
        {
            Displacement = new Displacement(),
            EngineAnalytics = new EngineAnalytics() { Displacement = 0 }
        };


        private string statusMessage = string.Empty;
        private string alertClass = string.Empty;

        private async Task HandleValidSubmit()
        {
            if (automobile.Displacement is not null)
            {
                automobile.EngineAnalytics = engineCalculationsService.CalculateEngineDisplacement(automobile.Displacement);
                await automobileDataService.AddAutomobile(automobile);
                statusMessage = "Successfully saved calculations";
                alertClass = "alert-success";
                StateHasChanged();  // required as the async nature post-await not updating the UI until next action
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
