namespace Ticketly.Application.Models;

public sealed class ServiceResult<T>
{
    private ServiceResult(T? value, ServiceErrorType errorType, string? errorMessage)
    {
        Value = value;
        ErrorType = errorType;
        ErrorMessage = errorMessage;
    }

    public T? Value { get; }

    public ServiceErrorType ErrorType { get; }

    public string? ErrorMessage { get; }

    public bool IsSuccess => ErrorType == ServiceErrorType.None;

    public static ServiceResult<T> Success(T value)
    {
        return new ServiceResult<T>(value, ServiceErrorType.None, null);
    }

    public static ServiceResult<T> BadRequest(string message)
    {
        return new ServiceResult<T>(default, ServiceErrorType.BadRequest, message);
    }

    public static ServiceResult<T> NotFound(string message)
    {
        return new ServiceResult<T>(default, ServiceErrorType.NotFound, message);
    }

    public static ServiceResult<T> Unauthorized(string message)
    {
        return new ServiceResult<T>(default, ServiceErrorType.Unauthorized, message);
    }
}
