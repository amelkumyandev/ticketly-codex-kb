using Ticketly.Api.Contracts;
using Ticketly.Application.Models;

namespace Ticketly.Api;

internal static class EndpointResultExtensions
{
    public static IResult ToEndpointResult<T>(this ServiceResult<T> result, Func<T, IResult> onSuccess)
    {
        if (result.IsSuccess && result.Value is not null)
        {
            return onSuccess(result.Value);
        }

        var message = result.ErrorMessage ?? "Request could not be completed.";

        return result.ErrorType switch
        {
            ServiceErrorType.BadRequest => Results.BadRequest(new ErrorResponse(message)),
            ServiceErrorType.NotFound => Results.NotFound(new ErrorResponse(message)),
            ServiceErrorType.Unauthorized => Results.Unauthorized(),
            _ => Results.Problem("An unexpected error occurred.")
        };
    }
}
