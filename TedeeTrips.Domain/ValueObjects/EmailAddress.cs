using CSharpFunctionalExtensions;

namespace TedeeTrips.Domain.ValueObjects;

public class EmailAddress : SimpleValueObject<string>
{
    private EmailAddress(string value) : base(value)
    {
    }
    
    private static bool ContainsAtSign(string value) => value.Contains('@');

    public static Result<EmailAddress, ErrorArray> Create(string value) =>
        Maybe.From(value)
             .ToResult(Errors.Email.InvalidValue().ToErrorArray())
             .Ensure(ContainsAtSign, _ => Errors.Email.InvalidValue())
             .Map(x => new EmailAddress(x));
}