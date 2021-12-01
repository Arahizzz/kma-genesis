using DataService;
using EventBus.RabbitMQ.Standard.Configuration;
using EventBus.RabbitMQ.Standard.Options;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the DI container.
{
    var services = builder.Services;

    //Database set up
    services.AddNpgsql<UserContext>(configuration.GetConnectionString("Database"));

    //RabbitMQ set up
    var rabbitMqOptions = configuration.GetSection("RabbitMq").Get<RabbitMqOptions>();
    services.AddRabbitMqConnection(rabbitMqOptions);
    services.AddRabbitMqRegistration(rabbitMqOptions);
    services.AddTransient<AddUserHandler>();

    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();
}

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

//REST API
app.MapGet("user", async (Guid id, UserContext db) =>
{
    var user = await db.Users.FindAsync(id);
    return user != null ? Results.Ok(user) : Results.NotFound();
});

app.SubscribeToEvents();

//Setup Database tables on launch
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<UserContext>();
    db.Database.EnsureCreated();
}

app.Run();