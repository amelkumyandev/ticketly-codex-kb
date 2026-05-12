using Microsoft.EntityFrameworkCore;
using Ticketly.Application.Services;
using Ticketly.Infrastructure.Persistence;

namespace Ticketly.Tests;

public sealed class TicketlyServiceTests
{
    [Fact]
    public async Task CreateEvent_WithValidData_Succeeds()
    {
        using var testServices = CreateServices();

        var result = await testServices.EventService.CreateAsync(
            "DotNet Community Day",
            "Yerevan Tech Hub",
            new DateTime(2026, 6, 1, 10, 0, 0, DateTimeKind.Utc),
            CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal("DotNet Community Day", result.Value.Name);
    }

    [Fact]
    public async Task CreateTicketType_ForExistingEvent_Succeeds()
    {
        using var testServices = CreateServices();
        var ticketlyEvent = await CreateEventAsync(testServices);

        var result = await testServices.TicketTypeService.CreateAsync(
            ticketlyEvent.Id,
            "General Admission",
            25m,
            "USD",
            100,
            CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(ticketlyEvent.Id, result.Value.EventId);
    }

    [Fact]
    public async Task CreateTicketType_SetsAvailableQuantityToTotalQuantity()
    {
        using var testServices = CreateServices();
        var ticketlyEvent = await CreateEventAsync(testServices);

        var result = await testServices.TicketTypeService.CreateAsync(
            ticketlyEvent.Id,
            "General Admission",
            25m,
            "USD",
            100,
            CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(result.Value.TotalQuantity, result.Value.AvailableQuantity);
    }

    [Fact]
    public async Task CreateReservation_WhenEnoughTicketsAvailable_Succeeds()
    {
        using var testServices = CreateServices();
        var ticketType = await CreateTicketTypeAsync(testServices, totalQuantity: 5);

        var result = await testServices.ReservationService.CreateAsync(
            ticketType.Id,
            2,
            "customer@example.com",
            CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(2, result.Value.Quantity);
    }

    [Fact]
    public async Task CreateReservation_WhenEnoughTicketsAvailable_ReducesAvailableQuantity()
    {
        using var testServices = CreateServices();
        var ticketType = await CreateTicketTypeAsync(testServices, totalQuantity: 5);

        await testServices.ReservationService.CreateAsync(
            ticketType.Id,
            2,
            "customer@example.com",
            CancellationToken.None);

        var updatedTicketType = await testServices.DbContext.TicketTypes
            .AsNoTracking()
            .SingleAsync(storedTicketType => storedTicketType.Id == ticketType.Id);

        Assert.Equal(3, updatedTicketType.AvailableQuantity);
    }

    [Fact]
    public async Task CreateReservation_WhenNotEnoughTicketsAvailable_Fails()
    {
        using var testServices = CreateServices();
        var ticketType = await CreateTicketTypeAsync(testServices, totalQuantity: 1);

        var result = await testServices.ReservationService.CreateAsync(
            ticketType.Id,
            2,
            "customer@example.com",
            CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.Equal("Not enough tickets available.", result.ErrorMessage);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task CreateReservation_WhenQuantityIsZeroOrNegative_Fails(int quantity)
    {
        using var testServices = CreateServices();
        var ticketType = await CreateTicketTypeAsync(testServices, totalQuantity: 5);

        var result = await testServices.ReservationService.CreateAsync(
            ticketType.Id,
            quantity,
            "customer@example.com",
            CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.Equal("Reservation quantity must be greater than zero.", result.ErrorMessage);
    }

    private static TestServices CreateServices()
    {
        var options = new DbContextOptionsBuilder<TicketlyDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var dbContext = new TicketlyDbContext(options);
        var repository = new TicketlyRepository(dbContext);

        return new TestServices(
            dbContext,
            new EventService(repository),
            new TicketTypeService(repository),
            new ReservationService(repository));
    }

    private static async Task<Ticketly.Application.Models.EventDto> CreateEventAsync(TestServices testServices)
    {
        var result = await testServices.EventService.CreateAsync(
            "DotNet Community Day",
            "Yerevan Tech Hub",
            new DateTime(2026, 6, 1, 10, 0, 0, DateTimeKind.Utc),
            CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);

        return result.Value;
    }

    private static async Task<Ticketly.Application.Models.TicketTypeDto> CreateTicketTypeAsync(
        TestServices testServices,
        int totalQuantity)
    {
        var ticketlyEvent = await CreateEventAsync(testServices);
        var result = await testServices.TicketTypeService.CreateAsync(
            ticketlyEvent.Id,
            "General Admission",
            25m,
            "USD",
            totalQuantity,
            CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);

        return result.Value;
    }

    private sealed record TestServices(
        TicketlyDbContext DbContext,
        EventService EventService,
        TicketTypeService TicketTypeService,
        ReservationService ReservationService) : IDisposable
    {
        public void Dispose()
        {
            DbContext.Dispose();
        }
    }
}
