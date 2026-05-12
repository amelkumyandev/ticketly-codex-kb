param(
    [Parameter(Mandatory = $true)]
    [string]$Token,

    [string]$ProjectKey = "ticketly-kb",

    [string]$SonarHostUrl = "http://localhost:9000",

    [string]$Configuration = "Debug"
)

if ([string]::IsNullOrWhiteSpace($Token)) {
    Write-Error "A SonarQube token is required. Create one in SonarQube and pass it with -Token."
    exit 1
}

Write-Host "Starting SonarQube analysis..."
Write-Host "Project key: $ProjectKey"
Write-Host "SonarQube URL: $SonarHostUrl"

$coverageDirectory = Join-Path (Get-Location) "TestResults"
$coverageOutputPrefix = Join-Path $coverageDirectory "coverage"
$coverageFile = Join-Path $coverageDirectory "coverage.opencover.xml"

New-Item -ItemType Directory -Force -Path $coverageDirectory | Out-Null

if (Test-Path $coverageFile) {
    Remove-Item $coverageFile
}

dotnet tool run dotnet-sonarscanner begin `
    /k:$ProjectKey `
    /d:sonar.host.url=$SonarHostUrl `
    /d:sonar.token=$Token `
    /d:sonar.cs.opencover.reportsPaths="**/coverage.opencover.xml"

if ($LASTEXITCODE -ne 0) {
    Write-Error "SonarScanner begin failed."
    exit $LASTEXITCODE
}

dotnet build --configuration $Configuration

if ($LASTEXITCODE -ne 0) {
    Write-Error "Build failed."
    exit $LASTEXITCODE
}

dotnet test `
    --configuration $Configuration `
    /p:CollectCoverage=true `
    /p:CoverletOutputFormat=opencover `
    /p:CoverletOutput="$coverageOutputPrefix"

if ($LASTEXITCODE -ne 0) {
    Write-Error "Tests failed."
    exit $LASTEXITCODE
}

if (-not (Test-Path $coverageFile)) {
    Write-Error "Coverage file was not generated at $coverageFile."
    exit 1
}

dotnet tool run dotnet-sonarscanner end `
    /d:sonar.token=$Token

if ($LASTEXITCODE -ne 0) {
    Write-Error "SonarScanner end failed."
    exit $LASTEXITCODE
}

Write-Host "SonarQube analysis completed."
Write-Host "Coverage file: $coverageFile"
