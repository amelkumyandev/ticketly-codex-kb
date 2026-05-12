using Ticketly.Application.Models;

namespace Ticketly.Application.Services;

public interface ITicketTypeService
{
    Task<ServiceResult<TicketTypeDto>> CreateAsync(
        Guid eventId,
        string? name,
        decimal price,
        string? currency,
        int totalQuantity,
        CancellationToken cancellationToken);

    Task<ServiceResult<IReadOnlyList<TicketTypeDto>>> ListForEventAsync(Guid eventId, CancellationToken cancellationToken);
}
