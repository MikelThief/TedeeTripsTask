using CSharpFunctionalExtensions;
using MediatR;
using TedeeTrips.Domain.Entities;

namespace TedeeTrips.Application.Query;

public record GetTrip(Guid Id) : IRequest<Maybe<Trip>>;