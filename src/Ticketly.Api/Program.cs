using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Ticketly.Api;
using Ticketly.Api.Contracts;
using Ticketly.Application.Services;
using Ticketly.Domain.Entities;
using Ticketly.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = GetRequiredConfigurationValue(builder.Configuration, "Jwt:Issuer"),
            ValidAudience = GetRequiredConfigurationValue(builder.Configuration, "Jwt:Audience"),
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(GetRequiredConfigurationValue(builder.Configuration, "Jwt:SigningKey")))
        };
    });
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole(UserRoles.Admin));
    options.AddPolicy("CustomerOrAdmin", policy => policy.RequireRole(UserRoles.Customer, UserRoles.Admin));
});
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<ITicketTypeService, TicketTypeService>();
builder.Services.AddScoped<IReservationService, ReservationService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/health", () => Results.Ok(new HealthResponse("Healthy")))
    .WithName("GetHealth");

app.MapPost(
        "/api/auth/register",
        async (RegisterRequest request, IAuthService authService, CancellationToken cancellationToken) =>
        {
            var result = await authService.RegisterAsync(
                request.Email,
                request.Password,
                request.Role,
                cancellationToken);

            return result.ToEndpointResult(user => Results.Created($"/api/users/{user.Id}", user));
        })
    .WithName("Register");

app.MapPost(
        "/api/auth/login",
        async (LoginRequest request, IAuthService authService, CancellationToken cancellationToken) =>
        {
            var result = await authService.LoginAsync(
                request.Email,
                request.Password,
                cancellationToken);

            return result.ToEndpointResult(Results.Ok);
        })
    .WithName("Login");

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
    .WithName("CreateEvent")
    .RequireAuthorization("AdminOnly");

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
    .WithName("CreateTicketType")
    .RequireAuthorization("AdminOnly");

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
    .WithName("CreateReservation")
    .RequireAuthorization("CustomerOrAdmin");

app.MapGet(
        "/api/reservations/{id:guid}",
        async (Guid id, IReservationService reservationService, CancellationToken cancellationToken) =>
        {
            var result = await reservationService.GetByIdAsync(id, cancellationToken);
            return result.ToEndpointResult(Results.Ok);
        })
    .WithName("GetReservationById")
    .RequireAuthorization("CustomerOrAdmin");

app.Run();

static string GetRequiredConfigurationValue(IConfiguration configuration, string key)
{
    var value = configuration[key];

    if (string.IsNullOrWhiteSpace(value))
    {
        throw new InvalidOperationException($"Configuration value '{key}' is required.");
    }

    return value;
}

internal sealed record HealthResponse(string Status);

public partial class Program;
