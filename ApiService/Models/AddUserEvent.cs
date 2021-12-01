using EventBus.Base.Standard;

namespace ApiService.Models;

public class AddUserEvent : IntegrationEvent
{
    public User User { get; set; }

    public AddUserEvent(User user)
    {
        User = user;
    }
}