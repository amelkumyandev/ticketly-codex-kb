using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Ticketly.Infrastructure.Persistence;

namespace Ticketly.Tests;

public sealed class AuthAddonTests
{
    [Fact]
    public async Task Register_WithValidData_Succeeds()
    {
        await using var application = new TicketlyApiFactory();
        var client = application.CreateClient();

        var response = await RegisterAsync(client, "admin@example.com", "Pass123$", "Admin");

        Assert.True(
            response.StatusCode == HttpStatusCode.Created,
            await response.Content.ReadAsStringAsync());

        var body = await response.Content.ReadFromJsonAsync<UserResponse>();
        Assert.NotNull(body);
        Assert.Equal("admin@example.com", body.Email);
        Assert.Equal("Admin", body.Role);
    }

    [Fact]
    public async Task Register_WithDuplicateEmail_Fails()
    {
        await using var application = new TicketlyApiFactory();
        var client = application.CreateClient();

        await RegisterAsync(client, "admin@example.com", "Pass123$", "Admin");
        var response = await RegisterAsync(client, "ADMIN@example.com", "Pass123$", "Admin");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Login_WithValidCredentials_ReturnsAccessToken()
    {
        await using var application = new TicketlyApiFactory();
        var client = application.CreateClient();

        await RegisterAsync(client, "customer@example.com", "Pass123$", "Customer");
        var token = await LoginAsync(client, "customer@example.com", "Pass123$");

        Assert.False(string.IsNullOrWhiteSpace(token));
    }

    [Fact]
    public async Task Login_WithInvalidCredentials_ReturnsUnauthorized()
    {
        await using var application = new TicketlyApiFactory();
        var client = application.CreateClient();

        await RegisterAsync(client, "customer@example.com", "Pass123$", "Customer");
        var response = await client.PostAsJsonAsync(
            "/api/auth/login",
            new LoginRequest("customer@example.com", "wrong-password"));

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task AdminOnlyEndpoint_WhenAnonymous_ReturnsUnauthorized()
    {
        await using var application = new TicketlyApiFactory();
        var client = application.CreateClient();

        var response = await CreateEventAsync(client);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task AdminOnlyEndpoint_WithCustomerRole_ReturnsForbidden()
    {
        await using var application = new TicketlyApiFactory();
        var client = application.CreateClient();
        var customerToken = await RegisterAndLoginAsync(client, "customer@example.com", "Customer");

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", customerToken);
        var response = await CreateEventAsync(client);

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task AdminOnlyEndpoint_WithAdminRole_Succeeds()
    {
        await using var application = new TicketlyApiFactory();
        var client = application.CreateClient();
        var adminToken = await RegisterAndLoginAsync(client, "admin@example.com", "Admin");

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", adminToken);
        var response = await CreateEventAsync(client);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task ReservationEndpoint_WhenAnonymous_ReturnsUnauthorized()
    {
        await using var application = new TicketlyApiFactory();
        var client = application.CreateClient();

        var response = await client.PostAsJsonAsync(
            "/api/reservations",
            new CreateReservationRequest(Guid.NewGuid(), 1, "customer@example.com"));

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task ReservationEndpoint_WithCustomerRole_Succeeds()
    {
        await using var application = new TicketlyApiFactory();
        var client = application.CreateClient();
        var adminToken = await RegisterAndLoginAsync(client, "admin@example.com", "Admin");
        var customerToken = await RegisterAndLoginAsync(client, "customer@example.com", "Customer");

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", adminToken);
        var eventResponse = await CreateEventAsync(client);
        var createdEvent = await eventResponse.Content.ReadFromJsonAsync<EventResponse>();
        Assert.NotNull(createdEvent);

        var ticketTypeResponse = await client.PostAsJsonAsync(
            $"/api/events/{createdEvent.Id}/ticket-types",
            new CreateTicketTypeRequest("General Admission", 25m, "USD", 10));
        var ticketType = await ticketTypeResponse.Content.ReadFromJsonAsync<TicketTypeResponse>();
        Assert.NotNull(ticketType);

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", customerToken);
        var reservationResponse = await client.PostAsJsonAsync(
            "/api/reservations",
            new CreateReservationRequest(ticketType.Id, 2, "customer@example.com"));

        Assert.Equal(HttpStatusCode.Created, reservationResponse.StatusCode);
    }

    private static async Task<HttpResponseMessage> RegisterAsync(
        HttpClient client,
        string email,
        string password,
        string role)
    {
        return await client.PostAsJsonAsync("/api/auth/register", new RegisterRequest(email, password, role));
    }

    private static async Task<string> RegisterAndLoginAsync(HttpClient client, string email, string role)
    {
        await RegisterAsync(client, email, "Pass123$", role);
        return await LoginAsync(client, email, "Pass123$");
    }

    private static async Task<string> LoginAsync(HttpClient client, string email, string password)
    {
        var response = await client.PostAsJsonAsync("/api/auth/login", new LoginRequest(email, password));

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var body = await response.Content.ReadFromJsonAsync<LoginResponse>();
        Assert.NotNull(body);

        return body.AccessToken;
    }

    private static async Task<HttpResponseMessage> CreateEventAsync(HttpClient client)
    {
        return await client.PostAsJsonAsync(
            "/api/events",
            new CreateEventRequest(
                "DotNet Community Day",
                "Yerevan Tech Hub",
                new DateTime(2026, 6, 1, 10, 0, 0, DateTimeKind.Utc)));
    }

    private sealed class TicketlyApiFactory : WebApplicationFactory<Program>
    {
        private readonly string databaseName = Guid.NewGuid().ToString();

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration((_, configurationBuilder) =>
            {
                configurationBuilder.AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["ConnectionStrings:DefaultConnection"] = "Host=localhost;Database=ticketly-tests",
                    ["Jwt:Issuer"] = "ticketly-tests",
                    ["Jwt:Audience"] = "ticketly-api-tests",
                    ["Jwt:SigningKey"] = "test-signing-key-for-ticketly-auth-addon-32",
                    ["Jwt:ExpiresMinutes"] = "60"
                });
            });

            builder.ConfigureServices(services =>
            {
                services.RemoveAll<DbContextOptions<TicketlyDbContext>>();
                services.RemoveAll<DbContextOptions>();
                services.RemoveAll<IDbContextOptionsConfiguration<TicketlyDbContext>>();
                services.RemoveAll<TicketlyDbContext>();
                services.AddDbContext<TicketlyDbContext>(options => options.UseInMemoryDatabase(databaseName));
            });
        }
    }

    private sealed record RegisterRequest(string Email, string Password, string Role);

    private sealed record LoginRequest(string Email, string Password);

    private sealed record CreateEventRequest(string Name, string Venue, DateTime StartsAt);

    private sealed record CreateTicketTypeRequest(string Name, decimal Price, string Currency, int TotalQuantity);

    private sealed record CreateReservationRequest(Guid TicketTypeId, int Quantity, string CustomerEmail);

    private sealed record UserResponse(Guid Id, string Email, string Role, DateTime CreatedAt);

    private sealed record LoginResponse(string AccessToken);

    private sealed record EventResponse(Guid Id, string Name, string Venue, DateTime StartsAt, DateTime CreatedAt);

    private sealed record TicketTypeResponse(
        Guid Id,
        Guid EventId,
        string Name,
        decimal Price,
        string Currency,
        int TotalQuantity,
        int AvailableQuantity,
        DateTime CreatedAt);
}
