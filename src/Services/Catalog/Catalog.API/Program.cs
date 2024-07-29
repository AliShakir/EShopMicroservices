

using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

//
var assembly = typeof(Program).Assembly;
// Add services to the container
builder.Services.AddCarter();
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssemblies(assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
});

builder.Services.AddValidatorsFromAssembly(assembly);

builder.Services.AddMarten(opts =>
{
    opts.Connection(builder.Configuration.GetConnectionString("conString")!);

}).UseLightweightSessions();

// Global Exception Handling...
builder.Services.AddExceptionHandler<CustomeExceptionHandler>();

// Health check...
builder.Services.AddHealthChecks()
    .AddNpgSql(builder.Configuration.GetConnectionString("conString")!);

// Configure the HTTP request pipline...
var app = builder.Build();

app.MapCarter();

app.UseExceptionHandler(optionns => { });

//
app.UseHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
});


app.Run();
