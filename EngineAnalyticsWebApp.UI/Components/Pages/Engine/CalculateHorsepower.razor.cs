using Microsoft.AspNetCore.Components;

namespace EngineAnalyticsWebApp.UI.Components.Pages.Engine
{
    // Modern .NET 8+ approach: Using [Route] attribute instead of @page directive
    // This keeps routing configuration in C# code-behind for type safety and consistency
    // Functionally equivalent to @page "/engine/calculate-horsepower" in .razor file
    // Multiple routes can be defined by stacking [Route] attributes
    [Route("/engine/calculate-horsepower")]
    public partial class CalculateHorsepower
    {
        private string title = "Engine Horsepower Calculation";
    }
}