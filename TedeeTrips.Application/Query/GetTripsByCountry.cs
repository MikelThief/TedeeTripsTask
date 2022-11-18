using MediatR;
using TedeeTrips.Domain.Entities;

namespace TedeeTrips.Application.Query;

public record GetTripsByCountry(string Country) : IRequest<ICollection<Trip>>;