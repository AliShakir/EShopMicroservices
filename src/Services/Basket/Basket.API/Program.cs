






using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container
var assembly = typeof(Program).Assembly;
//
builder.Services.AddCarter();
//
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssemblies(assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
});

// Marten Configuration...
builder.Services.AddMarten(opts =>
{
    opts.Connection(builder.Configuration.GetConnectionString("conString")!);
    opts.Schema.For<ShoppingCart>().Identity(x => x.UserName);
}).UseLightweightSessions();

//
builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.Decorate<IBasketRepository, CachedBasketRepository>();
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
});
//
builder.Services.AddExceptionHandler<CustomeExceptionHandler>();
//
builder.Services.AddHealthChecks()
    .AddNpgSql(builder.Configuration.GetConnectionString("conString")!)
    .AddRedis(builder.Configuration.GetConnectionString("Redis")!);


// Configure the HTTP request pipeline
var app = builder.Build();

//
app.MapCarter();

//
app.UseExceptionHandler(opt => { });

// 
app.UseHealthChecks("/health",new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});


app.Run();
