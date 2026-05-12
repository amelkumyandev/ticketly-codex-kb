# Ticketly Coding Standards

## Purpose

This document defines coding standards for the knowledge-base repository. The goal is clean, understandable, testable .NET code.

## General principles

- Prefer simple code over clever code.
- Optimize for readability.
- Keep methods small.
- Avoid duplication.
- Name things clearly.
- Fail fast on invalid input.
- Keep business logic outside controllers.
- Avoid unnecessary abstractions.
- Avoid unnecessary NuGet packages.

## C# standards

- Enable nullable reference types.
- Use async/await for I/O.
- Use `CancellationToken` in async service and endpoint methods where reasonable.
- Prefer constructor injection.
- Prefer immutable DTOs or records when appropriate.
- Use `decimal` for money.
- Use UTC timestamps.
- Use explicit access modifiers.

## Naming

Good names: `CreateEventRequest`, `CreateEventResponse`, `CreateTicketTypeRequest`, `CreateReservationRequest`, `ReservationService`, `TicketlyDbContext`.

Avoid vague names: `Manager`, `Helper`, `Processor`, `Data`, `Thing`, `ResultObj`.

## Controllers and endpoints

Controllers/endpoints should accept request DTOs, call application services, convert service results to HTTP responses, avoid direct EF Core logic, and avoid business decisions beyond HTTP mapping.

Controllers/endpoints should not directly manipulate ticket availability, contain complex validation logic, expose EF Core entities as responses, or swallow exceptions silently.

## Services

Application services should contain business workflows, validate important business rules, use `CancellationToken`, return clear result objects or controlled errors, and be easy to test.

## DTOs

Use DTOs for API boundaries. Do not return EF Core entities directly.

## Error handling

Use `400 Bad Request` for validation and business-rule failures, `404 Not Found` for missing resources, and `500 Internal Server Error` only for unexpected failures. Do not leak stack traces or connection strings.

## EF Core

Use PostgreSQL provider, migrations, clear relationships, async EF methods, and transactions if multiple writes must be consistent. Avoid lazy loading and raw SQL unless necessary.

Reservation creation should update availability consistently.

## Logging

Log important startup/configuration issues and unexpected exceptions if middleware exists. Do not log secrets, full customer email unnecessarily, or database passwords.

## Formatting

Before final result, run `dotnet format` if available. If unavailable, document it in `EXPERIMENT_LOG.md`.

## Anti-patterns to avoid

Business logic in controllers, EF entities as API responses, hardcoded connection strings in code, large methods, duplicate validation scattered across endpoints, tests that only check object creation but not behavior, and catching all exceptions and returning success.
