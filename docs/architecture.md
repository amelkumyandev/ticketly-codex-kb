# Ticketly Architecture Standard

## Purpose

This document defines the target architecture for the `ticketly-kb` repository. The goal is to keep the implementation simple enough for a 2-3 hour demo while still showing professional .NET engineering practices.

## Architecture style

Use a simple layered architecture.

Recommended solution structure:

```text
Ticketly.sln
src/
  Ticketly.Api/
  Ticketly.Application/
  Ticketly.Domain/
  Ticketly.Infrastructure/
tests/
  Ticketly.Tests/
```

If time is limited, `Ticketly.Tests` can contain both unit-style and integration-style tests. Do not create too many projects if it slows down the demo.

## Layer responsibilities

### Ticketly.Api

Responsible for HTTP endpoints or controllers, request DTOs, response DTOs, dependency injection setup, configuration binding, OpenAPI/Swagger setup, and error response mapping.

Not responsible for business rules, EF Core queries directly in endpoints, database transaction logic, or domain state changes beyond mapping requests to use cases.

### Ticketly.Application

Responsible for use cases, application services, business workflow coordination, input validation orchestration, and application-level results.

Examples: `EventService`, `TicketTypeService`, `ReservationService`.

### Ticketly.Domain

Responsible for core entities and domain-level invariants.

Entities: `Event`, `TicketType`, `Reservation`.

Domain rules: ticket quantity cannot be negative, reservation quantity must be greater than zero, reservation cannot reduce availability below zero, ticket type initial availability equals total quantity.

### Ticketly.Infrastructure

Responsible for EF Core `DbContext`, entity configuration, database migrations, PostgreSQL persistence, and data access implementation if used.

## Data model

### Event

Required fields: `Id`, `Name`, `Venue`, `StartsAt`, `CreatedAt`.

### TicketType

Required fields: `Id`, `EventId`, `Name`, `Price`, `Currency`, `TotalQuantity`, `AvailableQuantity`, `CreatedAt`.

Rules: belongs to one event, price cannot be negative, total quantity must be greater than zero, available quantity initially equals total quantity.

### Reservation

Required fields: `Id`, `TicketTypeId`, `Quantity`, `CustomerEmail`, `CreatedAt`.

Rules: belongs to one ticket type, quantity must be greater than zero, requires customer email, creating a reservation reduces ticket availability.

## Database

Use PostgreSQL with EF Core. Local development should use Docker Compose.

Connection string should be configurable:

```text
ConnectionStrings__DefaultConnection=Host=postgres;Port=5432;Database=ticketly;Username=ticketly;Password=ticketly
```

## API endpoints

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

## Design constraints

Do not add frontend, authentication, authorization, payment processing, email notifications, background jobs, distributed messaging, microservices, complex DDD tactical patterns, or CQRS/MediatR unless already present and clearly useful.

## Good implementation signs

Controllers or endpoints are thin, business rules are testable without HTTP, DTOs are separate from EF entities, tests cover meaningful behavior, Docker Compose starts PostgreSQL, SonarQube setup is documented, and token-burn tracking is updated.
