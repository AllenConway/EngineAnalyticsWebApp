using EngineAnalyticsWebApp.Shared.Models.Engine;
using EngineAnalyticsWebApp.Shared.Services.Data;

namespace EngineAnalyticsWebApp.Components.Reporting
{
    public partial class TorqueDataGrid(IAutomobileDataService AutomobileDataService)
    {
        private IEnumerable<Automobile> automobileData = new List<Automobile>();

        protected override async Task OnInitializedAsync()
        {
            automobileData = await AutomobileDataService.GetAutomobiles();
            automobileData = automobileData.Where(x => x.EngineAnalytics?.Torque != 0).ToList();
        }
    }
}
