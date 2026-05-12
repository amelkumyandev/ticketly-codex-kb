using Ticketly.Application.Models;

namespace Ticketly.Application.Services;

public interface IEventService
{
    Task<ServiceResult<EventDto>> CreateAsync(string? name, string? venue, DateTime startsAt, CancellationToken cancellationToken);

    Task<ServiceResult<IReadOnlyList<EventDto>>> ListAsync(CancellationToken cancellationToken);

    Task<ServiceResult<EventDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}
