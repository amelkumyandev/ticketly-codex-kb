using Ticketly.Application.Abstractions;
using Ticketly.Application.Models;
using Ticketly.Domain.Entities;

namespace Ticketly.Application.Services;

public sealed class EventService(ITicketlyRepository repository) : IEventService
{
    public async Task<ServiceResult<EventDto>> CreateAsync(
        string? name,
        string? venue,
        DateTime startsAt,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return ServiceResult<EventDto>.BadRequest("Event name is required.");
        }

        if (string.IsNullOrWhiteSpace(venue))
        {
            return ServiceResult<EventDto>.BadRequest("Event venue is required.");
        }

        var ticketlyEvent = Event.Create(name, venue, startsAt, DateTime.UtcNow);

        await repository.AddEventAsync(ticketlyEvent, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return ServiceResult<EventDto>.Success(ToDto(ticketlyEvent));
    }

    public async Task<ServiceResult<IReadOnlyList<EventDto>>> ListAsync(CancellationToken cancellationToken)
    {
        var events = await repository.ListEventsAsync(cancellationToken);
        return ServiceResult<IReadOnlyList<EventDto>>.Success(events.Select(ToDto).ToList());
    }

    public async Task<ServiceResult<EventDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var ticketlyEvent = await repository.GetEventByIdAsync(id, cancellationToken);

        return ticketlyEvent is null
            ? ServiceResult<EventDto>.NotFound("Event was not found.")
            : ServiceResult<EventDto>.Success(ToDto(ticketlyEvent));
    }

    private static EventDto ToDto(Event ticketlyEvent)
    {
        return new EventDto(
            ticketlyEvent.Id,
            ticketlyEvent.Name,
            ticketlyEvent.Venue,
            ticketlyEvent.StartsAt,
            ticketlyEvent.CreatedAt);
    }
}
