using Ticketly.Application.Abstractions;
using Ticketly.Application.Models;
using Ticketly.Domain.Entities;

namespace Ticketly.Application.Services;

public sealed class ReservationService(ITicketlyRepository repository) : IReservationService
{
    public async Task<ServiceResult<ReservationDto>> CreateAsync(
        Guid ticketTypeId,
        int quantity,
        string? customerEmail,
        CancellationToken cancellationToken)
    {
        if (quantity <= 0)
        {
            return ServiceResult<ReservationDto>.BadRequest("Reservation quantity must be greater than zero.");
        }

        if (string.IsNullOrWhiteSpace(customerEmail))
        {
            return ServiceResult<ReservationDto>.BadRequest("Customer email is required.");
        }

        var ticketType = await repository.GetTicketTypeByIdAsync(ticketTypeId, trackChanges: true, cancellationToken);

        if (ticketType is null)
        {
            return ServiceResult<ReservationDto>.NotFound("Ticket type was not found.");
        }

        if (quantity > ticketType.AvailableQuantity)
        {
            return ServiceResult<ReservationDto>.BadRequest("Not enough tickets available.");
        }

        ticketType.Reserve(quantity);
        var reservation = Reservation.Create(ticketType.Id, quantity, customerEmail, DateTime.UtcNow);

        await repository.AddReservationAsync(reservation, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return ServiceResult<ReservationDto>.Success(ToDto(reservation));
    }

    public async Task<ServiceResult<ReservationDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var reservation = await repository.GetReservationByIdAsync(id, cancellationToken);

        return reservation is null
            ? ServiceResult<ReservationDto>.NotFound("Reservation was not found.")
            : ServiceResult<ReservationDto>.Success(ToDto(reservation));
    }

    private static ReservationDto ToDto(Reservation reservation)
    {
        return new ReservationDto(
            reservation.Id,
            reservation.TicketTypeId,
            reservation.Quantity,
            reservation.CustomerEmail,
            reservation.CreatedAt);
    }
}
