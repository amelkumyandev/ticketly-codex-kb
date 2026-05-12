# Ticketly

Ticketly is a small .NET 10 ASP.NET Core Web API for event ticket reservation.

This repository is part of the `ticketly-kb` experiment and follows the knowledge-base guidance in `AGENTS.md` and `docs/`.

## Current Scope

The current implementation includes the Ticketly API, JWT auth, tests, coverage, and local SonarQube support:

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
- JWT register/login endpoints
- Role-based authorization for Admin and Customer users
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
$env:Jwt__Issuer = "ticketly-local"
$env:Jwt__Audience = "ticketly-api"
$env:Jwt__SigningKey = "local-demo-signing-key-change-for-real-use-32"
$env:Jwt__ExpiresMinutes = "60"
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
POST /api/auth/register
POST /api/auth/login
POST /api/events
GET  /api/events
GET  /api/events/{id}
POST /api/events/{eventId}/ticket-types
GET  /api/events/{eventId}/ticket-types
POST /api/reservations
GET  /api/reservations/{id}
```

Public endpoints:

```text
GET  /health
POST /api/auth/register
POST /api/auth/login
GET  /api/events
GET  /api/events/{id}
GET  /api/events/{eventId}/ticket-types
```

Admin endpoints:

```text
POST /api/events
POST /api/events/{eventId}/ticket-types
```

Customer or Admin endpoints:

```text
POST /api/reservations
GET  /api/reservations/{id}
```

## JWT Authentication

JWT configuration is read from configuration or environment variables:

```text
Jwt__Issuer=ticketly-local
Jwt__Audience=ticketly-api
Jwt__SigningKey=local-demo-signing-key-change-for-real-use-32
Jwt__ExpiresMinutes=60
```

The signing key shown here is for local demo use only. Use a strong secret from a secure configuration source outside source control for any real deployment.

Register a user:

```http
POST /api/auth/register
Content-Type: application/json

{
  "email": "admin@example.com",
  "password": "Pass123$",
  "role": "Admin"
}
```

Supported roles:

```text
Admin
Customer
```

Login:

```http
POST /api/auth/login
Content-Type: application/json

{
  "email": "admin@example.com",
  "password": "Pass123$"
}
```

Use the returned token on protected endpoints:

```http
Authorization: Bearer YOUR_ACCESS_TOKEN
```

Example Admin flow:

```powershell
$adminRegister = @{
  email = "admin@example.com"
  password = "Pass123$"
  role = "Admin"
} | ConvertTo-Json

Invoke-RestMethod -Method Post -Uri http://localhost:5089/api/auth/register -ContentType "application/json" -Body $adminRegister

$adminLogin = @{
  email = "admin@example.com"
  password = "Pass123$"
} | ConvertTo-Json

$adminToken = (Invoke-RestMethod -Method Post -Uri http://localhost:5089/api/auth/login -ContentType "application/json" -Body $adminLogin).accessToken

$headers = @{ Authorization = "Bearer $adminToken" }

$event = @{
  name = "DotNet Community Day"
  venue = "Yerevan Tech Hub"
  startsAt = "2026-06-01T10:00:00Z"
} | ConvertTo-Json

Invoke-RestMethod -Method Post -Uri http://localhost:5089/api/events -Headers $headers -ContentType "application/json" -Body $event
```

Example Customer flow:

```powershell
$customerRegister = @{
  email = "customer@example.com"
  password = "Pass123$"
  role = "Customer"
} | ConvertTo-Json

Invoke-RestMethod -Method Post -Uri http://localhost:5089/api/auth/register -ContentType "application/json" -Body $customerRegister

$customerLogin = @{
  email = "customer@example.com"
  password = "Pass123$"
} | ConvertTo-Json

$customerToken = (Invoke-RestMethod -Method Post -Uri http://localhost:5089/api/auth/login -ContentType "application/json" -Body $customerLogin).accessToken

$headers = @{ Authorization = "Bearer $customerToken" }

$reservation = @{
  ticketTypeId = "00000000-0000-0000-0000-000000000000"
  quantity = 2
  customerEmail = "customer@example.com"
} | ConvertTo-Json

Invoke-RestMethod -Method Post -Uri http://localhost:5089/api/reservations -Headers $headers -ContentType "application/json" -Body $reservation
```

Example create event request:

```http
POST /api/events
Authorization: Bearer ADMIN_ACCESS_TOKEN
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
Authorization: Bearer ADMIN_ACCESS_TOKEN
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
Authorization: Bearer CUSTOMER_OR_ADMIN_ACCESS_TOKEN
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

Docker Compose uses local demo credentials and JWT settings only.

## Demo Security Statement

This project is a demo. It implements basic JWT authentication and role-based authorization, but intentionally does not implement payment handling, production-grade identity management, refresh tokens, account recovery, email verification, rate limiting, or production-grade security hardening.
