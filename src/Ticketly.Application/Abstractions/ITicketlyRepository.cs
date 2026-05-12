using Ticketly.Domain.Entities;

namespace Ticketly.Application.Abstractions;

public interface ITicketlyRepository
{
    Task AddEventAsync(Event ticketlyEvent, CancellationToken cancellationToken);

    Task<IReadOnlyList<Event>> ListEventsAsync(CancellationToken cancellationToken);

    Task<Event?> GetEventByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<bool> EventExistsAsync(Guid id, CancellationToken cancellationToken);

    Task AddTicketTypeAsync(TicketType ticketType, CancellationToken cancellationToken);

    Task<IReadOnlyList<TicketType>> ListTicketTypesForEventAsync(Guid eventId, CancellationToken cancellationToken);

    Task<TicketType?> GetTicketTypeByIdAsync(Guid id, bool trackChanges, CancellationToken cancellationToken);

    Task AddReservationAsync(Reservation reservation, CancellationToken cancellationToken);

    Task<Reservation?> GetReservationByIdAsync(Guid id, CancellationToken cancellationToken);

    Task SaveChangesAsync(CancellationToken cancellationToken);
}
