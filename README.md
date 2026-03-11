This solution currently requires .NET 8 and 9 SDKs for building as DartSassBuilder requires .NET 8
[See the following for .NET SDK installations](https://dotnet.microsoft.com/en-us/download/visual-studio-sdks)

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
