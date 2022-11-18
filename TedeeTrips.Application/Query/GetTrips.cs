using MediatR;
using TedeeTrips.Domain.Entities;

namespace TedeeTrips.Application.Query;

public record GetTrips() : IRequest<ICollection<Trip>>;