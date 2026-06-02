param(
    [string]$ResultsDirectory = "TestResults",
    [string]$ReportDirectory = "CoverageReport"
)

$ErrorActionPreference = 'Stop'

Write-Host "Running tests with coverage..."
dotnet test --settings coverlet.runsettings --results-directory $ResultsDirectory

$reportGenerator = Join-Path $env:USERPROFILE ".dotnet\tools\reportgenerator.exe"
if (-not (Test-Path $reportGenerator)) {
    Write-Host "Installing reportgenerator global tool..."
    dotnet tool install -g dotnet-reportgenerator-globaltool | Out-Null
}

Write-Host "Generating coverage reports..."
& $reportGenerator "-reports:$ResultsDirectory/**/coverage.cobertura.xml" "-targetdir:$ReportDirectory" "-reporttypes:Html;TextSummary"
