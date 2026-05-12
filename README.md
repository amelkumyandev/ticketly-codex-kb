# Ticketly

Ticketly is a small .NET 10 ASP.NET Core Web API for event ticket reservation.

This repository is part of the `ticketly-kb` experiment and follows the knowledge-base guidance in `AGENTS.md` and `docs/`.

## Current Scope

Task 3 implements the Ticketly API, tests, coverage, and local SonarQube support:

- .NET 10 solution named `Ticketly`
- ASP.NET Core Web API project
- Application, Domain, and Infrastructure class library projects
- xUnit test project
- Dockerfile for the API
- Docker Compose PostgreSQL service
- Health endpoint at `GET /health`
- PostgreSQL persistence through EF Core
- EF Core migration support
- Event, ticket type, and reservation endpoints
- Business behavior tests for creation and reservation rules
- OpenCover test coverage output
- Local SonarQube Docker Compose support

## Project Structure

```text
Ticketly.slnx
src/
  Ticketly.Api/
  Ticketly.Application/
  Ticketly.Domain/
  Ticketly.Infrastructure/
tests/
  Ticketly.Tests/
```

## Run Locally

```powershell
dotnet build
dotnet test
$env:ConnectionStrings__DefaultConnection = "Host=localhost;Port=5432;Database=ticketly;Username=ticketly;Password=ticketly"
dotnet run --project .\src\Ticketly.Api\Ticketly.Api.csproj
```

Health check:

```powershell
Invoke-RestMethod http://localhost:5089/health
```

The local launch profile uses port `5089`. Docker Compose exposes the API on port `8080`.

OpenAPI JSON is available in development at:

```text
http://localhost:5089/openapi/v1.json
```

## Run with Docker Compose

```powershell
docker compose up --build
```

Health check:

```powershell
Invoke-RestMethod http://localhost:8080/health
```

Stop services:

```powershell
docker compose down
```

## Database Migrations

This repository uses a local EF Core tool manifest.

Restore local tools:

```powershell
dotnet tool restore
```

Add a migration:

```powershell
dotnet tool run dotnet-ef migrations add MigrationName --project .\src\Ticketly.Infrastructure\Ticketly.Infrastructure.csproj --startup-project .\src\Ticketly.Api\Ticketly.Api.csproj --output-dir Persistence\Migrations
```

Apply migrations:

```powershell
$env:ConnectionStrings__DefaultConnection = "Host=localhost;Port=5432;Database=ticketly;Username=ticketly;Password=ticketly"
dotnet tool run dotnet-ef database update --project .\src\Ticketly.Infrastructure\Ticketly.Infrastructure.csproj --startup-project .\src\Ticketly.Api\Ticketly.Api.csproj
```

## Tests and Coverage

Run tests:

```powershell
dotnet test
```

Generate an OpenCover report:

```powershell
.\scripts\run-tests-with-coverage.ps1
```

If local PowerShell execution policy blocks unsigned scripts, run:

```powershell
powershell -ExecutionPolicy Bypass -File .\scripts\run-tests-with-coverage.ps1
```

Coverage output:

```text
TestResults/coverage.opencover.xml
```

SonarQube can consume the report with this pattern:

```text
**/coverage.opencover.xml
```

## SonarQube Local Analysis

Start SonarQube and its PostgreSQL database:

```powershell
docker compose -f docker-compose.sonarqube.yml up -d
```

Open SonarQube:

```text
http://localhost:9000
```

Use the default local login:

```text
Username: admin
Password: admin
```

SonarQube will prompt for a new password on first login. After logging in, create a user token from **My Account > Security**.

Restore local .NET tools:

```powershell
dotnet tool restore
```

Run analysis:

```powershell
.\scripts\run-sonarqube-analysis.ps1 -Token "YOUR_TOKEN_HERE"
```

If local PowerShell execution policy blocks unsigned scripts, run:

```powershell
powershell -ExecutionPolicy Bypass -File .\scripts\run-sonarqube-analysis.ps1 -Token "YOUR_TOKEN_HERE"
```

The analysis script runs SonarScanner begin, `dotnet build`, `dotnet test` with OpenCover output, and SonarScanner end. It sends coverage to SonarQube using:

```text
**/coverage.opencover.xml
```

Stop SonarQube:

```powershell
docker compose -f docker-compose.sonarqube.yml down
```

## API Endpoints

```text
GET  /health
POST /api/events
GET  /api/events
GET  /api/events/{id}
POST /api/events/{eventId}/ticket-types
GET  /api/events/{eventId}/ticket-types
POST /api/reservations
GET  /api/reservations/{id}
```

Example create event request:

```http
POST /api/events
Content-Type: application/json

{
  "name": "DotNet Community Day",
  "venue": "Yerevan Tech Hub",
  "startsAt": "2026-06-01T10:00:00Z"
}
```

Example create ticket type request:

```http
POST /api/events/{eventId}/ticket-types
Content-Type: application/json

{
  "name": "General Admission",
  "price": 25.00,
  "currency": "USD",
  "totalQuantity": 100
}
```

Example create reservation request:

```http
POST /api/reservations
Content-Type: application/json

{
  "ticketTypeId": "00000000-0000-0000-0000-000000000000",
  "quantity": 2,
  "customerEmail": "customer@example.com"
}
```

## Configuration

The PostgreSQL connection string is configured through environment variables:

```text
ConnectionStrings__DefaultConnection=Host=postgres;Port=5432;Database=ticketly;Username=ticketly;Password=ticketly
```

Docker Compose uses local demo credentials only.

## Demo Security Statement

This project is a demo and intentionally does not implement authentication, authorization, payment handling, or production-grade security hardening.
