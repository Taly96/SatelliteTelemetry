using SatelliteTelemetryAPI.Core.Interfaces;
using SatelliteTelemetryAPI.Core.Services;
using SatelliteTelemetryAPI.Infrastructure.Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSingleton<ITelemetryRepository, InMemoryTelemetryRepository>();
builder.Services.AddScoped<ITelemetryService, SatelliteTelemetryService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.Run();