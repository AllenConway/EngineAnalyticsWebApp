using EngineAnalyticsWebApp.Shared.Models.Engine;
using EngineAnalyticsWebApp.Shared.Services.Data;
using Microsoft.AspNetCore.Components;

namespace EngineAnalyticsWebApp.Components.Reporting
{
    public partial class HorsepowerDataGrid
    {

        [Inject]
        private IAutomobileDataService AutomobileDataService { get; set; } = default!;

        [Parameter]
        public RenderFragment<Automobile> RowTemplate { get; set; } = default!;

        [Parameter]
        public RenderFragment? TableHeader { get; set; } = default!;

        private IEnumerable<Automobile> automobileData = new List<Automobile>();
        private IEnumerable<Automobile> filteredAutomobileData = new List<Automobile>();

        /// <summary>
        /// Initializes the component and loads the automobile data.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            await LoadAutomobileData();
        }

        /// <summary>
        /// Loads the automobile data from the data service and applies the initial filter.
        /// </summary>
        private async Task LoadAutomobileData()
        {
            automobileData = await AutomobileDataService.GetAutomobiles();
            automobileData = automobileData.Where(x => x.EngineAnalytics?.RearWheelHorsepower != 0).ToList();
            ApplyModelFilter(null); // No filter on initialization
        }

        /// <summary>
        /// Handles the filter applied event and updates the filtered automobile data.
        /// </summary>
        /// <param name="newFilter">The new filter string.</param>
        private void HandleFilterApplied(string? newFilter)
        {
            ApplyModelFilter(newFilter);
        }

        /// <summary>
        /// Applies the model filter to the automobile data.
        /// </summary>
        /// <param name="filter">The filter string to apply to the Model property. If null or empty, no filtering is applied.</param>
        private void ApplyModelFilter(string? filter)
        {
            if (string.IsNullOrEmpty(filter))
            {
                filteredAutomobileData = automobileData;
            }
            else
            {
                // Currently filtering on the Model property: 'ApplyModelFilter'
                filteredAutomobileData = automobileData.Where(auto => auto?.Model?.ToString().Contains(filter, StringComparison.OrdinalIgnoreCase) == true).ToList();
            }
        }
    }
}