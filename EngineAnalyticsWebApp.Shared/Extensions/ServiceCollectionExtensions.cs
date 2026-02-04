using Microsoft.Extensions.DependencyInjection;

namespace EngineAnalyticsWebApp.Shared.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds validation services for types defined in the Shared project.
    /// This is required for nested object validation when models are in a different assembly.
    /// See: https://learn.microsoft.com/en-us/aspnet/core/release-notes/aspnetcore-10.0?view=aspnetcore-10.0#use-validation-models-from-a-different-assembly
    /// </summary>
    public static IServiceCollection AddValidationForSharedTypes(
        this IServiceCollection collection)
    {
        return collection.AddValidation();
    }
}
