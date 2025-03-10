using Microsoft.AspNetCore.Components;

namespace EngineAnalyticsWebApp.Components.Filters
{
    public partial class HorsepowerFilter
    {
        [Parameter]
        public EventCallback<string> OnFilterApplied { get; set; }

        [Parameter]
        public required string FilterDescription { get; set; }

        private string? filter;

        private async Task ApplyFilter()
        {
            if (!string.IsNullOrEmpty(filter))
            {
                await OnFilterApplied.InvokeAsync(filter);
            }
        }

        private async Task ClearFilter()
        {
            filter = null;
            await OnFilterApplied.InvokeAsync(null);
        }
    }
}
