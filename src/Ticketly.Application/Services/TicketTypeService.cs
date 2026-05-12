using Ticketly.Application.Abstractions;
using Ticketly.Application.Models;
using Ticketly.Domain.Entities;

namespace Ticketly.Application.Services;

public sealed class TicketTypeService(ITicketlyRepository repository) : ITicketTypeService
{
    public async Task<ServiceResult<TicketTypeDto>> CreateAsync(
        Guid eventId,
        string? name,
        decimal price,
        string? currency,
        int totalQuantity,
        CancellationToken cancellationToken)
    {
        if (!await repository.EventExistsAsync(eventId, cancellationToken))
        {
            return ServiceResult<TicketTypeDto>.NotFound("Event was not found.");
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            return ServiceResult<TicketTypeDto>.BadRequest("Ticket type name is required.");
        }

        if (price < 0)
        {
            return ServiceResult<TicketTypeDto>.BadRequest("Ticket type price cannot be negative.");
        }

        if (string.IsNullOrWhiteSpace(currency))
        {
            return ServiceResult<TicketTypeDto>.BadRequest("Ticket type currency is required.");
        }

        if (totalQuantity <= 0)
        {
            return ServiceResult<TicketTypeDto>.BadRequest("Ticket type total quantity must be greater than zero.");
        }

        var ticketType = TicketType.Create(eventId, name, price, currency, totalQuantity, DateTime.UtcNow);

        await repository.AddTicketTypeAsync(ticketType, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return ServiceResult<TicketTypeDto>.Success(ToDto(ticketType));
    }

    public async Task<ServiceResult<IReadOnlyList<TicketTypeDto>>> ListForEventAsync(
        Guid eventId,
        CancellationToken cancellationToken)
    {
        if (!await repository.EventExistsAsync(eventId, cancellationToken))
        {
            return ServiceResult<IReadOnlyList<TicketTypeDto>>.NotFound("Event was not found.");
        }

        var ticketTypes = await repository.ListTicketTypesForEventAsync(eventId, cancellationToken);
        return ServiceResult<IReadOnlyList<TicketTypeDto>>.Success(ticketTypes.Select(ToDto).ToList());
    }

    private static TicketTypeDto ToDto(TicketType ticketType)
    {
        return new TicketTypeDto(
            ticketType.Id,
            ticketType.EventId,
            ticketType.Name,
            ticketType.Price,
            ticketType.Currency,
            ticketType.TotalQuantity,
            ticketType.AvailableQuantity,
            ticketType.CreatedAt);
    }
}
