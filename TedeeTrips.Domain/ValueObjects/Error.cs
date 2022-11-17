using CSharpFunctionalExtensions;
using System.Diagnostics.CodeAnalysis;

namespace TedeeTrips.Domain.ValueObjects;

public class Error : ValueObject
{
    public Error(string code, string description)
    {
        Code = code;
        Description = description;
    }

    public string Code { get; }

    public string Description { get; }

    public static implicit operator ErrorArray(Error e) => new ErrorArray(e);

    public ErrorArray ToErrorArray() => new ErrorArray(this);

    [ExcludeFromCodeCoverage]
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Code;
    }
}