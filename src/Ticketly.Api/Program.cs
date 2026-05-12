using Ticketly.Api;
using Ticketly.Api.Contracts;
using Ticketly.Application.Services;
using Ticketly.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<ITicketTypeService, TicketTypeService>();
builder.Services.AddScoped<IReservationService, ReservationService>();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapGet("/health", () => Results.Ok(new HealthResponse("Healthy")))
    .WithName("GetHealth");

app.MapPost(
        "/api/events",
        async (CreateEventRequest request, IEventService eventService, CancellationToken cancellationToken) =>
        {
            var result = await eventService.CreateAsync(
                request.Name,
                request.Venue,
                request.StartsAt,
                cancellationToken);

            return result.ToEndpointResult(createdEvent =>
                Results.Created($"/api/events/{createdEvent.Id}", createdEvent));
        })
    .WithName("CreateEvent");

app.MapGet(
        "/api/events",
        async (IEventService eventService, CancellationToken cancellationToken) =>
        {
            var result = await eventService.ListAsync(cancellationToken);
            return result.ToEndpointResult(Results.Ok);
        })
    .WithName("ListEvents");

app.MapGet(
        "/api/events/{id:guid}",
        async (Guid id, IEventService eventService, CancellationToken cancellationToken) =>
        {
            var result = await eventService.GetByIdAsync(id, cancellationToken);
            return result.ToEndpointResult(Results.Ok);
        })
    .WithName("GetEventById");

app.MapPost(
        "/api/events/{eventId:guid}/ticket-types",
        async (
            Guid eventId,
            CreateTicketTypeRequest request,
            ITicketTypeService ticketTypeService,
            CancellationToken cancellationToken) =>
        {
            var result = await ticketTypeService.CreateAsync(
                eventId,
                request.Name,
                request.Price,
                request.Currency,
                request.TotalQuantity,
                cancellationToken);

            return result.ToEndpointResult(ticketType =>
                Results.Created($"/api/events/{eventId}/ticket-types", ticketType));
        })
    .WithName("CreateTicketType");

app.MapGet(
        "/api/events/{eventId:guid}/ticket-types",
        async (Guid eventId, ITicketTypeService ticketTypeService, CancellationToken cancellationToken) =>
        {
            var result = await ticketTypeService.ListForEventAsync(eventId, cancellationToken);
            return result.ToEndpointResult(Results.Ok);
        })
    .WithName("ListTicketTypesForEvent");

app.MapPost(
        "/api/reservations",
        async (
            CreateReservationRequest request,
            IReservationService reservationService,
            CancellationToken cancellationToken) =>
        {
            var result = await reservationService.CreateAsync(
                request.TicketTypeId,
                request.Quantity,
                request.CustomerEmail,
                cancellationToken);

            return result.ToEndpointResult(reservation =>
                Results.Created($"/api/reservations/{reservation.Id}", reservation));
        })
    .WithName("CreateReservation");

app.MapGet(
        "/api/reservations/{id:guid}",
        async (Guid id, IReservationService reservationService, CancellationToken cancellationToken) =>
        {
            var result = await reservationService.GetByIdAsync(id, cancellationToken);
            return result.ToEndpointResult(Results.Ok);
        })
    .WithName("GetReservationById");

app.Run();

internal sealed record HealthResponse(string Status);
