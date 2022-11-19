using MediatR;

namespace TedeeTrips.Domain.Commands;

public record DeleteTrip(Guid Id) : IRequest;