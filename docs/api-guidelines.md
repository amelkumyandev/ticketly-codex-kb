# Ticketly API Guidelines

## Required endpoints

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

## HTTP status codes

Use `200 OK` for successful reads, `201 Created` for successful creates, `400 Bad Request` for validation failures and business-rule failures, `404 Not Found` for missing resources, and `500 Internal Server Error` only for unexpected failures.

## Request and response DTOs

Use DTOs. Do not expose EF Core entities directly.

Recommended requests: `CreateEventRequest`, `CreateTicketTypeRequest`, `CreateReservationRequest`.

Recommended responses: `EventResponse`, `TicketTypeResponse`, `ReservationResponse`.

## Validation rules

Event: `Name` is required, `Venue` is required, and `StartsAt` should be valid.

Ticket type: event must exist, `Name` is required, `Price` cannot be negative, `Currency` is required, `TotalQuantity` must be greater than zero, and `AvailableQuantity` initially equals `TotalQuantity`.

Reservation: ticket type must exist, `Quantity` must be greater than zero, `CustomerEmail` is required, reservation fails if not enough tickets are available, and reservation reduces `AvailableQuantity`.

## Error response style

Use clear and consistent error messages. A simple error response is acceptable:

```json
{ "message": "Not enough tickets available." }
```

ProblemDetails is also acceptable if implemented consistently. Do not return raw exception text.

## OpenAPI

Enable Swagger/OpenAPI in development. README should explain how to open Swagger UI.

Expected local URL: `http://localhost:8080/swagger` or the actual URL used by the project.

## API examples

Create event:

```http
POST /api/events
Content-Type: application/json

{
  "name": "DotNet Community Day",
  "venue": "Yerevan Tech Hub",
  "startsAt": "2026-06-01T10:00:00Z"
}
```

Create ticket type:

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

Create reservation:

```http
POST /api/reservations
Content-Type: application/json

{
  "ticketTypeId": "00000000-0000-0000-0000-000000000000",
  "quantity": 2,
  "customerEmail": "customer@example.com"
}
```
