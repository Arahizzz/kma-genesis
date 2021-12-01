using ApiService.Models;

namespace ApiService.Services;

public interface IUserQueryService
{
    Task<User> GetUser(Guid id);
}

public class UserQueryService : IUserQueryService
{
    private readonly HttpClient _client;

    public UserQueryService(HttpClient client)
    {
        _client = client;
    }

    public async Task<User> GetUser(Guid id)
    {
        var result = await _client.GetAsync($"?id={id}");
        result.EnsureSuccessStatusCode();
        return (await result.Content.ReadFromJsonAsync<User>())!;
    }
}