using Bogus;
using CSharpFunctionalExtensions;
using FluentAssertions;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using TedeeTrips.Domain;
using TedeeTrips.Domain.ValueObjects;
using Xunit;

namespace TedeeTrips.Tests.Unit.Domain.ValueObjects;

public class ErrorArrayTests
{
    [Fact]
    public void ErrorArray_Is_Empty_When_Created()
    {
        var firstArray = new ErrorArray();
        var secondArray = ErrorArray.Empty;


        firstArray.AsEnumerable().Should().BeEmpty();
        secondArray.AsEnumerable().Should().BeEmpty();
    }

    [Fact]
    public void ErrorArray_Has_Multiple_Errors_When_Created_From_Array_Of_Errors()
    {
        var immutableArrayOfErrors = ImmutableArray<Error>.Empty
                                                          .Add(Errors.Country.InvalidValue())
                                                          .Add(Errors.Email.InvalidValue());

        var errorArayFromImutableAray = new ErrorArray(immutableArrayOfErrors);


        errorArayFromImutableAray.AsEnumerable().Should().BeEquivalentTo(immutableArrayOfErrors);
    }

    [Fact]
    public void ErrorArray_Has_An_Error_When_Created_From_Error()
    {
        var error = new Faker().PickRandom(Errors.Country.InvalidValue(), Errors.Email.InvalidValue());
        var errorArray = new ErrorArray(error);


        errorArray.AsEnumerable().Should().HaveCount(1);
        errorArray.AsEnumerable().Should().Contain(error);
    }

    [Fact]
    public void ErrorArray_Is_A_Copy_If_Created_From_Other_ErrorArray()
    {
        var source = new ErrorArray(Errors.Email.InvalidValue());

        var errorArray = new ErrorArray(source);

        errorArray.AsEnumerable().Should().BeEquivalentTo(source);
    }

    [Fact]
    public void ErrorArray_Creates_A_New_Array_If_Errors_Were_Added()
    {
        var firstError = Errors.Email.InvalidValue();
        var secondError = Errors.Country.InvalidValue();
        var source = ErrorArray.Empty;
        var errorArray = source.With(firstError, secondError);


        errorArray.AsEnumerable().Should().Contain(firstError);
        errorArray.AsEnumerable().Should().Contain(secondError);
    }

    [Fact]
    public void ErrorArray_Creates_A_New_Array_If_List_Of_Errors_Was_Added()
    {
        var errors = new List<Error>()
        {
            Errors.Email.InvalidValue(),
            Errors.Country.InvalidValue()
        };

        var source = ErrorArray.Empty;
        var errorArray = source.With(errors);


        errorArray.AsEnumerable().Should().BeEquivalentTo(errors);
    }

    [Fact]
    public void ErrorArray_Doesn_not_contain_Error_From_Successful_Result()
    {
        var result = Result.Success<bool, ErrorArray>(true);
        var source = ErrorArray.Empty;
        var errorArray = source.With(result);


        errorArray.AsEnumerable().Should().BeEmpty();
    }

    [Fact]
    public void ErrorArray_Contains_Errors_From_Failed_Result()
    {
        var error = Errors.Email.InvalidValue();
        var result = Result.Failure<bool, ErrorArray>(error);
        var source = ErrorArray.Empty;
        var erroryArray = source.With(result);


        erroryArray.AsEnumerable().Should().Contain(error);
    }

    [Fact]
    public void ErorArray_Combines_With_IEnumerable_Or_Self()
    {
        var source = new ErrorArray(Errors.Country.InvalidValue());
        var errorArray = (ErrorArray) ErrorArray.Empty.Combine(source);


        errorArray.AsEnumerable().Should().BeEquivalentTo(source);
    }

    [Fact]
    public void ErrorArray_Is_The_Same_As_Other_ErrorArray_If_Number_Of_Errors_And_Codes_Are_The_Same()
    {
        var firstArray = Errors.Email.InvalidValue().ToErrorArray().With(Errors.Country.InvalidValue());
        var secondArray = Errors.Country.InvalidValue().ToErrorArray().With(Errors.Email.InvalidValue());


        firstArray.AsEnumerable().Should().BeEquivalentTo(secondArray);
    }
}