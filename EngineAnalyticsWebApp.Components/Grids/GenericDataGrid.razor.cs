
using Microsoft.AspNetCore.Components;

namespace EngineAnalyticsWebApp.Components.Grids
{
    public partial class GenericDataGrid<TItem>
    {
        [Parameter] public IEnumerable<TItem> Data { get; set; }
        [Parameter] public TItem? SelectedRow { get; set; }
        [Parameter] public RenderFragment? Columns { get; set; }

    }
}
