# EngineAnalyticsWebApp

`EngineAnalyticsWebApp` is a .NET 10 Blazor WebAssembly standalone application for engine performance analytics and weather tracking. It combines reusable Razor components, shared engine/weather models, and client-side services to calculate and display horsepower, torque, displacement, and weather data in a browser-based UI.

## Overview

This solution is organized as a Blazor-first workspace with shared UI and data logic split across multiple projects:

- `EngineAnalyticsWebApp.UI` — the standalone Blazor WebAssembly host and user-facing pages
- `EngineAnalyticsWebApp.Components` — reusable UI components, calculations, reporting, and weather components
- `EngineAnalyticsWebApp.Shared` — shared models, validation helpers, and client-side data services
- `EngineAnalyticsWebApp.TestLazy` — lazily loaded messaging feature used by the UI
- `*.Tests` projects — unit and component tests for the solution's services and Blazor components

The app is designed to:

- calculate engine horsepower, torque, and displacement
- store and retrieve vehicle data locally in the browser
- display report-style tables and filters for engine analytics results
- track current and future weather information by ZIP code
- demonstrate Blazor WebAssembly lazy-loading and component composition

## Build requirements

This solution targets .NET 10. Building also requires the .NET 8 SDK because `DartSassBuilder` depends on it.
[See the following for .NET SDK installations](https://dotnet.microsoft.com/en-us/download/visual-studio-sdks)

## Unit Testing

### Frameworks used

- `xUnit` for standard unit tests across services and models
- `bUnit` for Blazor component tests in the `Components.Tests`, `Shared.Tests`, and `UI.Tests` projects

### Why these frameworks were chosen

- `xUnit` is a lightweight, widely used .NET testing framework that fits well with service, model, and utility tests.
- `bUnit` is the recommended approach for testing Blazor components because it can render components and verify markup, events, and component behavior in a real Blazor test context.
- This combination keeps the tests focused, readable, and aligned with the solution's Blazor/WebAssembly architecture.

### Running unit tests and coverage

Use the included PowerShell script from the repository root:

```powershell
./scripts/coverage.ps1
```

That script runs the full test suite with coverage enabled, then generates an HTML coverage report and a text summary.

If you want to run the commands manually, the script performs the equivalent of:

```powershell
dotnet test --settings coverlet.runsettings --results-directory TestResults
```

and then:

```powershell
reportgenerator "-reports:TestResults/**/coverage.cobertura.xml" "-targetdir:CoverageReport" "-reporttypes:Html;TextSummary"
```

After it finishes:

- Open `CoverageReport/index.htm` for the HTML report
- Review `CoverageReport/Summary.txt` for a quick coverage summary

The current solution coverage is above 85%.

## OpenWeatherMap API Key Setup

The weather feature uses the [OpenWeatherMap API](https://openweathermap.org/). The API key is **not committed to source control** and must be configured locally before running the app.

### 1. Get a free API key

1. Sign up for a free account at [https://openweathermap.org/](https://openweathermap.org/)
2. After signing in, navigate to **API keys** under your profile
3. Copy your default key or generate a new one (the free tier supports the *Current Weather* endpoint used here)

> **Note:** New keys can take up to a few hours to activate.

### 2. Configure the key locally

Create the file `EngineAnalyticsWebApp.UI/wwwroot/appsettings.Development.json` (already listed in `.gitignore` — it will never be committed):

```json
{
  "OpenWeatherMap": {
    "ApiKey": "YOUR_API_KEY_HERE"
  }
}
```

This file is loaded automatically when running in the `Development` environment (the default for `dotnet run` / Visual Studio F5). The committed `appsettings.json` contains an empty placeholder and is safe to commit.

### 3. Configure the key for Azure (production)

Create the file `EngineAnalyticsWebApp.UI/wwwroot/appsettings.Production.json` (also gitignored) with the same structure:

```json
{
  "OpenWeatherMap": {
    "ApiKey": "YOUR_API_KEY_HERE"
  }
}
```

Blazor WASM runs entirely in the browser — Azure App Service "Application Settings" (portal environment variables) are server-side only and **are not accessible to the WASM runtime**. The `appsettings.Production.json` file is the correct equivalent: Blazor automatically loads it when the environment is `Production` (the Azure default), and Visual Studio Publish includes it in the `wwwroot` output automatically.

> **Do not use** Azure Key Vault or App Service Configuration for this value unless a server-side proxy component is added to the solution.
