using Marten.Events.Projections;
using Marten.Events;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddCarter();
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssemblies(typeof(Program).Assembly);
});
builder.Services.AddMarten(opts =>
{
    opts.Connection(builder.Configuration.GetConnectionString("conString")!);
    
}).UseLightweightSessions();
var app = builder.Build();

// Configure the HTTP request pipline...
app.MapCarter();

app.Run();
