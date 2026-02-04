using Microsoft.Extensions.Validation;
using System.ComponentModel.DataAnnotations;

namespace EngineAnalyticsWebApp.Shared.Models.Engine
{
    /// <summary>
    /// Represents an automobile with engine analytics data.
    /// </summary>
    /// <summary>
    /// Represents an automobile with engine analytics data.
    /// The <see cref="ValidatableTypeAttribute"/> enables .NET 10's enhanced nested object validation,
    /// allowing <see cref="DataAnnotationsValidator"/> to recursively validate complex properties
    /// like <see cref="Horsepower"/>, <see cref="Displacement"/>, and <see cref="Torque"/>.
    /// 
    /// Note: This attribute is marked experimental (ASP0029) in .NET 10 to provide API flexibility.
    /// Expected to be stable in .NET 11 (November 2026). See: https://aka.ms/aspnet/analyzer/ASP0029
    /// </summary>
    [ValidatableType]
    public class Automobile
    {
        [Required]
        public int? Year { get; set; }
        [Required]
        public string? Make { get; set; }
        [Required]
        public string? Model { get; set; }
        public Horsepower? Horsepower { get; set; }
        public Displacement? Displacement { get; set; }
        public Torque? Torque { get; set; }
        public EngineAnalytics? EngineAnalytics { get; set; }
    }
}
