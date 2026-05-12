using Ticketly.Application.Models;

namespace Ticketly.Application.Services;

public interface IReservationService
{
    Task<ServiceResult<ReservationDto>> CreateAsync(
        Guid ticketTypeId,
        int quantity,
        string? customerEmail,
        CancellationToken cancellationToken);

    Task<ServiceResult<ReservationDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}
