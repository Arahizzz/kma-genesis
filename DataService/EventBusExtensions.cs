using DataService.Models;
using EventBus.Base.Standard;

namespace DataService;

public static class EventBusExtensions
{
    public static IApplicationBuilder SubscribeToEvents(this IApplicationBuilder app)
    {
        var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();

        eventBus.Subscribe<AddUserEvent, AddUserHandler>();

        return app;
    }
}