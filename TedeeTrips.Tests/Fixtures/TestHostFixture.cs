using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using Xunit;

namespace TedeeTrips.Tests.Fixtures;

public class TestHostFixture : WebApplicationFactory<Program>
{

}

[CollectionDefinition(nameof(ApiCollection))]
public class ApiCollection : ICollectionFixture<TestHostFixture> { }