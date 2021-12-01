using DataService.Models;
using EventBus.Base.Standard;

namespace DataService;

public class AddUserHandler : IIntegrationEventHandler<AddUserEvent>
{
    private readonly ILogger<AddUserHandler> _logger;
    private readonly UserContext _db;

    public AddUserHandler(UserContext db, ILogger<AddUserHandler> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task Handle(AddUserEvent @event)
    {
        try
        {
            await _db.AddUser(@event.User);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while adding user");
        }
    }
}