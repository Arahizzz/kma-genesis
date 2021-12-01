using EventBus.Base.Standard;

namespace Messages;

public class AddUserEvent : IntegrationEvent
{
    public AddUserEvent(Guid id, string? name, string? surname, string? email)
    {
        Id = id;
        Name = name;
        Surname = surname;
        Email = email;
    }
}