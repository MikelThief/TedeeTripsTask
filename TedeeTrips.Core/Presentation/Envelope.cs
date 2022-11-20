using System.Text.Json.Serialization;
using TedeeTrips.Domain.ValueObjects;

namespace TedeeTrips.Core.Presentation;

public class Envelope<T>
{
    public T? Result { get; }
    
    public string? ErrorCode { get; }
    public string? ErrorMessage { get; }
    public DateTimeOffset CreatedAt { get; }

    [JsonConstructor]
    public Envelope(T? result, string? errorCode, string? errorMessage)
    {
        Result = result;
        ErrorCode = errorCode;
        ErrorMessage = errorMessage;
        CreatedAt = DateTimeOffset.UtcNow;
    }
}

public sealed class Envelope : Envelope<string>
{
    public Envelope(string errorCode, string errorMessage)
        : base(null, errorCode, errorMessage)
    {
    }

    public static Envelope<T> Ok<T>(T result)
    {
        return new Envelope<T>(result, null, null);
    }

    public static Envelope Ok()
    {
        return new Envelope(null!, null!);
    }

    public static Envelope Error(ErrorArray errors)
    {
        return new Envelope(string.Join("|", errors.Select(e => e.Code)), string.Join("|", errors.Select(e => e.Description)));
    }

    public static Envelope Error(string errorCode, string errorMessage)
    {
        return new Envelope(errorCode, errorMessage);
    }
}