# AGENTS.md - Ticketly Knowledge-Base Instructions

You are working in the `ticketly-kb` repository.

This repository is part of an experiment comparing Codex output with and without a project knowledge base.

## Required behavior

Before making code changes, read these files:

1. `docs/architecture.md`
2. `docs/coding-standards.md`
3. `docs/api-guidelines.md`
4. `docs/qa-standards.md`
5. `docs/security-standards.md`
6. `docs/sonarqube-standards.md`
7. `docs/token-burn-tracking.md`
8. `docs/comparison-summary.md`

## Project goal

Build the exact same application scope as the plain repository: a small .NET 10 Web API called `Ticketly` for event ticket reservation.

Required stack: .NET 10, ASP.NET Core Web API, PostgreSQL, Entity Framework Core, Docker Compose, xUnit, test coverage, SonarQube local analysis, Windows PowerShell scripts, and token-burn tracking.

## Application scope

Implement only these features:

1. Create event
2. List events
3. Get event by id
4. Create ticket type for event
5. List ticket types for event
6. Reserve tickets
7. Get reservation by id

Do not add frontend, authentication, authorization, payments, emails, background workers, reservation confirmation, reservation cancellation, or expiration logic.

## Architecture rules

Follow a simple, professional layered architecture:

- API layer: HTTP endpoints/controllers, request/response DTOs, dependency injection, configuration.
- Application layer: use cases, services, business rules, validation orchestration.
- Domain layer: domain entities and domain-level invariants.
- Infrastructure layer: EF Core DbContext, persistence, database configuration.

If the repository currently has fewer projects, create the minimum clean structure needed. Avoid overengineering.

## Coding rules

- Keep controllers thin.
- Do not put business rules directly in controllers.
- Use async APIs.
- Pass `CancellationToken` where reasonable.
- Use DTOs for API input/output.
- Do not expose EF Core entities directly from API responses.
- Use meaningful names.
- Avoid duplicate code.
- Keep methods small and readable.
- Avoid unnecessary packages.
- Do not introduce abstractions without a clear benefit.

## API rules

- Return `201 Created` for successful create operations.
- Return `200 OK` for successful reads.
- Return `404 Not Found` for missing resources.
- Return `400 Bad Request` for validation and business-rule failures.
- Use clear error messages.
- Do not leak internal exception details.

## Testing rules

- Add tests for business behavior.
- Include success and failure paths.
- Use xUnit.
- Prefer readable test names.
- Generate coverage report.
- Keep tests deterministic.

Required behavior tests:

- Event creation succeeds with valid data.
- Ticket type creation succeeds for an existing event.
- Ticket type `AvailableQuantity` initially equals `TotalQuantity`.
- Reservation succeeds when enough tickets are available.
- Reservation reduces `AvailableQuantity`.
- Reservation fails when not enough tickets are available.
- Reservation fails when quantity is zero or negative.

## PostgreSQL rules

- Use PostgreSQL through Docker Compose.
- Use EF Core migrations.
- Connection string should be configurable through environment variables.
- Do not hardcode secrets in source code beyond local demo defaults in Docker Compose.

## SonarQube rules

- Add local SonarQube Docker Compose support.
- Add PowerShell scripts for analysis.
- Add README instructions.
- Keep code quality high: low duplication, no obvious dead code, no obvious security smells, no large unreadable methods, no avoidable warnings.

## Token-burn tracking rules

You must update `TOKEN_BURN.md`, `EXPERIMENT_LOG.md`, and `EXPERIMENT_RESULT.md`.

For each task:

1. Record the prompt or task text as input.
2. Record what you produced as output summary.
3. If exact platform token counts are available, record them.
4. If exact platform token counts are not available, run `scripts/estimate-token-burn.ps1`.
5. Clearly label the result as `exact platform usage` or `estimated local usage`.

Do not skip token-burn tracking.

## Required commands before finishing each implementation task

```powershell
dotnet build
dotnet test
```

When coverage is available, also run:

```powershell
.\scriptsun-tests-with-coverage.ps1
```

When SonarQube is configured and token is available, run:

```powershell
.\scriptsun-sonarqube-analysis.ps1 -Token "<token>"
```

If a command cannot be run in the current environment, document the reason in `EXPERIMENT_LOG.md`.

## Definition of done

A task is done only when code compiles, tests pass or failures are documented, README is updated when setup changes, token tracking is updated, experiment log is updated, and assumptions are documented.
