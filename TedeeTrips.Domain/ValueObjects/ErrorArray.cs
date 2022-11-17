using CSharpFunctionalExtensions;
using System.Collections;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

namespace TedeeTrips.Domain.ValueObjects;

public sealed class ErrorArray : ValueObject, IReadOnlyCollection<Error>, ICombine
{
    private ImmutableArray<Error> _errors;

    public ErrorArray() => _errors = ImmutableArray<Error>.Empty;

    public ErrorArray(Error error) => _errors = ImmutableArray.Create(error);

    public ErrorArray(ImmutableArray<Error> errors) => _errors = errors;

    public ErrorArray(ErrorArray errors) => _errors = ImmutableArray<Error>.Empty.AddRange(errors);

    public static ErrorArray Empty => new ErrorArray();

    public int Count => _errors.Length;

    public IEnumerator<Error> GetEnumerator() => ((IEnumerable<Error>)_errors).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_errors).GetEnumerator();

    public ErrorArray With(Error error) => new ErrorArray(_errors.Add(error));

    public ErrorArray With(params Error[] errors) => new ErrorArray(_errors.AddRange(errors));

    public ErrorArray With(IEnumerable<Error> errors) => new ErrorArray(_errors.AddRange(errors));

    public ErrorArray With<T>(Result<T, ErrorArray> result)
    {
        if (result.IsFailure)
        {
            return new ErrorArray(_errors.AddRange(result.Error));
        }

        return this;
    }

    public ICombine Combine(ICombine value)
    {
        if (value is IEnumerable<Error> otherErrors)
        {
            return With(otherErrors);
        }
        else
        {
            throw new ArgumentException(nameof(ErrorArray) + $" does not support combining with types other than {nameof(IEnumerable<Error>)}.");
        }
    }

    [ExcludeFromCodeCoverage]
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Count;
        for (int cnt = 0; cnt < Count; cnt++)
        {
            yield return _errors[cnt];
        }
    }
}