using CSharpFunctionalExtensions;

namespace TedeeTrips.Domain.ValueObjects;

public class EmailAddress : SimpleValueObject<string>
{
    private EmailAddress(string value) : base(value)
    {
    }
    
    private static bool ContainsAtSign(string value) => value.Contains("@");

    public static Result<EmailAddress, ErrorArray> Create(string value) =>
        Maybe.From(value)
             .ToResult(Errors.Email.InvalidValue())
             .Ensure(x => ContainsAtSign(x), _ => Errors.Email.InvalidValue())
             .Finally(x => new EmailAddress(x.Value));
}