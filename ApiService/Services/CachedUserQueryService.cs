using ApiService.Models;
using Microsoft.Extensions.Caching.Distributed;

namespace ApiService.Services;

public class CachedUserQueryService : IUserQueryService
{
    private readonly IDistributedCache _cache;
    private readonly IUserQueryService _userService;
    private readonly ILogger<CachedUserQueryService> _logger;


    public CachedUserQueryService(IDistributedCache cache, IUserQueryService userService, ILogger<CachedUserQueryService> logger)
    {
        _cache = cache;
        _userService = userService;
        _logger = logger;
    }

    public async Task<User> GetUser(Guid id)
    {
        User? user = await _cache.GetValue<User>(id.ToString());
        if (user == null)
        {
            _logger.LogInformation("User is not cached. Loading from data service...");
            user = await _userService.GetUser(id);
            await _cache.SetValue(id.ToString(), user);
        }
        else
        {
            _logger.LogInformation("Returning cached user");
        }

        return user;
    }
}