using System.Net;
using ApiService.Models;
using ApiService.Services;
using EventBus.Base.Standard;
using EventBus.RabbitMQ.Standard.Configuration;
using EventBus.RabbitMQ.Standard.Options;
using Microsoft.AspNetCore.Diagnostics;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the DI container.
{
    var services = builder.Services;
    //Set up Redis cache
    services.AddStackExchangeRedisCache(opt =>
    {
        opt.Configuration = configuration.GetConnectionString("Redis");
        opt.InstanceName = "User_";
    });

    //RabbitMQ set up
    var rabbitMqOptions = configuration.GetSection("RabbitMq").Get<RabbitMqOptions>();
    services.AddRabbitMqConnection(rabbitMqOptions);
    services.AddRabbitMqRegistration(rabbitMqOptions);

    //Services set up
    services.AddHttpClient<IUserQueryService, UserQueryService>(client =>
        client.BaseAddress = new Uri("http://data:80/user"));
    services.Decorate<IUserQueryService, CachedUserQueryService>();

    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();
}

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();
app.UseExceptionHandler("/error");

//REST API
app.MapPost("user", (User user, IEventBus eventBus) =>
{
    var @event = new AddUserEvent(user);
    eventBus.Publish(@event);
});
app.MapGet("user", async (Guid id, IUserQueryService userQuery) => await userQuery.GetUser(id));

//Handle exceptions from REST calls to Data Service
app.MapGet("/error", async ctx =>
{
    var context = ctx.Features.Get<IExceptionHandlerFeature>();
    var code = HttpStatusCode.InternalServerError;
    var exception = context?.Error;

    if (exception is HttpRequestException http) code = http.StatusCode ?? HttpStatusCode.InternalServerError;

    ctx.Response.StatusCode = (int) code;
    await ctx.Response.WriteAsJsonAsync(new { exception?.Message });
    await ctx.Response.CompleteAsync();
});

app.Run();