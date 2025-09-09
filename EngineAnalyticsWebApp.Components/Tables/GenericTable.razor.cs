

using Microsoft.AspNetCore.Components;

namespace EngineAnalyticsWebApp.Components.Tables
{
    public partial class GenericTable<TItem>
    {
        [Parameter] public IEnumerable<TItem> Items { get; set; } = [];
        [Parameter] public RenderFragment? HeaderCells { get; set; }
        [Parameter] public RenderFragment<TItem>? RowCells { get; set; }
    }
}
