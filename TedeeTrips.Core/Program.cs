using TedeeTrips.Application.DI;
using TedeeTrips.Infrastructure.DI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services
       .AddDomainLayer()
       .AddApplicationLayer()
       .AddInfrastructureLayer();

builder.Services
       .AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseDeveloperExceptionPage();
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();

#pragma warning disable CA1050 // Declare types in namespaces
public partial class Program
{
}
#pragma warning restore CA1050 // Declare types in namespaces