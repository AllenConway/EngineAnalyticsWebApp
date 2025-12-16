using EngineAnalyticsWebApp.Shared.Models.Engine;
using EngineAnalyticsWebApp.Shared.Services.Data;

namespace EngineAnalyticsWebApp.Components.Reporting
{
    public partial class DisplacementDataGrid(IAutomobileDataService automobileDataService)
    {
        private IEnumerable<Automobile> automobileData = new List<Automobile>();
        private Automobile? selectedAutomobile;

        protected override async Task OnInitializedAsync()
        {
            automobileData = await automobileDataService.GetAutomobiles();
            automobileData = automobileData.Where(x => x.EngineAnalytics?.Displacement != 0).ToList();
        }
    }
}
