param(
    [string]$Configuration = "Debug"
)

Write-Host "Running tests with coverage..."

$coverageDirectory = Join-Path (Get-Location) "TestResults"
$coverageOutputPrefix = Join-Path $coverageDirectory "coverage"
$coverageFile = Join-Path $coverageDirectory "coverage.opencover.xml"

New-Item -ItemType Directory -Force -Path $coverageDirectory | Out-Null

if (Test-Path $coverageFile) {
    Remove-Item $coverageFile
}

dotnet test `
    --configuration $Configuration `
    /p:CollectCoverage=true `
    /p:CoverletOutputFormat=opencover `
    /p:CoverletOutput="$coverageOutputPrefix"

if ($LASTEXITCODE -ne 0) {
    Write-Error "Tests with coverage failed."
    exit $LASTEXITCODE
}

if (-not (Test-Path $coverageFile)) {
    Write-Error "Coverage file was not generated at $coverageFile."
    exit 1
}

$coverageXml = [xml](Get-Content $coverageFile -Raw)
$summary = $coverageXml.CoverageSession.Summary
$visitedSequencePoints = [decimal]$summary.visitedSequencePoints
$sequencePoints = [decimal]$summary.numSequencePoints
$lineCoverage = 0

if ($sequencePoints -gt 0) {
    $lineCoverage = [math]::Round(($visitedSequencePoints / $sequencePoints) * 100, 2)
}

Write-Host "Coverage command completed."
Write-Host "Coverage file: $coverageFile"
Write-Host "Line coverage: $lineCoverage%"
Write-Host "SonarQube coverage pattern: **/coverage.opencover.xml"
