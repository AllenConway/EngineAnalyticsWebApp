using EngineAnalyticsWebApp.Components.Reporting;
using Microsoft.AspNetCore.Components;

namespace EngineAnalyticsWebApp.UI.Components.Pages.Reports
{
    public partial class DynamicReports
    {
        private Type? selectedReportType = null;
        private Dictionary<string, object> reportParameters = new();

        private void OnReportChanged(ChangeEventArgs e)
        {
            var value = e.Value?.ToString();
            selectedReportType = value switch
            {
                "Displacement" => typeof(DisplacementDataGrid),
                "Torque"       => typeof(TorqueDataGrid),
                _              => null
            };
            reportParameters.Clear();
        }
    }
}
