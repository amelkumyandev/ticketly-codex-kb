using Microsoft.EntityFrameworkCore;
using Ticketly.Application.Abstractions;
using Ticketly.Domain.Entities;

namespace Ticketly.Infrastructure.Persistence;

public sealed class TicketlyRepository(TicketlyDbContext dbContext) : ITicketlyRepository
{
    public async Task AddEventAsync(Event ticketlyEvent, CancellationToken cancellationToken)
    {
        await dbContext.Events.AddAsync(ticketlyEvent, cancellationToken);
    }

    public async Task<IReadOnlyList<Event>> ListEventsAsync(CancellationToken cancellationToken)
    {
        return await dbContext.Events
            .AsNoTracking()
            .OrderBy(ticketlyEvent => ticketlyEvent.StartsAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<Event?> GetEventByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await dbContext.Events
            .AsNoTracking()
            .FirstOrDefaultAsync(ticketlyEvent => ticketlyEvent.Id == id, cancellationToken);
    }

    public async Task<bool> EventExistsAsync(Guid id, CancellationToken cancellationToken)
    {
        return await dbContext.Events.AnyAsync(ticketlyEvent => ticketlyEvent.Id == id, cancellationToken);
    }

    public async Task AddTicketTypeAsync(TicketType ticketType, CancellationToken cancellationToken)
    {
        await dbContext.TicketTypes.AddAsync(ticketType, cancellationToken);
    }

    public async Task<IReadOnlyList<TicketType>> ListTicketTypesForEventAsync(
        Guid eventId,
        CancellationToken cancellationToken)
    {
        return await dbContext.TicketTypes
            .AsNoTracking()
            .Where(ticketType => ticketType.EventId == eventId)
            .OrderBy(ticketType => ticketType.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<TicketType?> GetTicketTypeByIdAsync(
        Guid id,
        bool trackChanges,
        CancellationToken cancellationToken)
    {
        var query = trackChanges
            ? dbContext.TicketTypes
            : dbContext.TicketTypes.AsNoTracking();

        return await query.FirstOrDefaultAsync(ticketType => ticketType.Id == id, cancellationToken);
    }

    public async Task AddReservationAsync(Reservation reservation, CancellationToken cancellationToken)
    {
        await dbContext.Reservations.AddAsync(reservation, cancellationToken);
    }

    public async Task<Reservation?> GetReservationByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await dbContext.Reservations
            .AsNoTracking()
            .FirstOrDefaultAsync(reservation => reservation.Id == id, cancellationToken);
    }

    public async Task AddUserAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        await dbContext.Users.AddAsync(user, cancellationToken);
    }

    public async Task<ApplicationUser?> GetUserByEmailAsync(string email, CancellationToken cancellationToken)
    {
        return await dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(user => user.Email == email, cancellationToken);
    }

    public async Task<bool> UserEmailExistsAsync(string email, CancellationToken cancellationToken)
    {
        return await dbContext.Users.AnyAsync(user => user.Email == email, cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
