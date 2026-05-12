# Ticketly SonarQube Standards

## Purpose

SonarQube is used to compare code quality between `ticketly-plain` and `ticketly-kb`.

## Local SonarQube setup

Use Docker Compose with `sonarqube:lts-community` and `postgres:17` for the SonarQube database.

Recommended file: `docker-compose.sonarqube.yml`.

## Required scripts

Create:

```text
scripts/run-tests-with-coverage.ps1
scripts/run-sonarqube-analysis.ps1
```

## SonarQube analysis script

The script should accept `Token`, `ProjectKey`, and `SonarHostUrl`.

Example:

```powershell
.\scriptsun-sonarqube-analysis.ps1 -Token "YOUR_TOKEN_HERE"
```

The script should run `dotnet sonarscanner begin`, `dotnet build`, `dotnet test` with coverage, and `dotnet sonarscanner end`.

## Coverage path

Prefer `**/coverage.opencover.xml`.

## README requirements

README must explain how to start SonarQube, open SonarQube, use the default login, create a token, install `dotnet-sonarscanner`, run analysis, and view results.

## Suggested commands

```powershell
docker compose -f docker-compose.sonarqube.yml up -d
docker compose -f docker-compose.sonarqube.yml down
dotnet tool install --global dotnet-sonarscanner
.\scriptsun-sonarqube-analysis.ps1 -Token "YOUR_TOKEN_HERE"
```

## Quality metrics to record

Record bugs, vulnerabilities, security hotspots, code smells, duplicated lines percentage, coverage percentage, maintainability rating, reliability rating, and security rating in `EXPERIMENT_RESULT.md`.

## Demo acceptance

Acceptable: Docker Compose exists, scripts exist, README has clear commands, project can be analyzed locally when SonarQube is running, and execution limitations are documented.

Not acceptable: no SonarQube setup, no analysis instructions, no coverage path, or no comparison notes.
