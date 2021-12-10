using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ApiService.Models;
using FluentAssertions;
using NUnit.Framework;

namespace Testing;

public class IntegrationTests
{
    private readonly HttpClient _client = new HttpClient
    {
        BaseAddress = new Uri(Environment.GetEnvironmentVariable("API_URL") ?? "http://localhost:7170/")
    };

    [Test]
    public async Task GetUnknownUserNotFound()
    {
        var randomId = Guid.NewGuid();
        
        var response = await _client.GetAsync($"user?id={randomId}");
        
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Test]
    public async Task PostUserReturns()
    {
        var id = Guid.NewGuid();
        var user = new User
        {
            Id = id,
            Email = "abc@email.com",
            Name = "Name",
            Surname = "Surname"
        };
        
        var content = JsonContent.Create(user);
        var postResponse = await _client.PostAsync("user", content);
        postResponse.StatusCode.Should().BeOneOf(HttpStatusCode.OK, HttpStatusCode.Created);

        await Task.Delay(1000);
        
        var getResponse = await _client.GetAsync($"user?id={id}");
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var responseUser = await getResponse.Content.ReadFromJsonAsync<User>();
        responseUser.Should().BeEquivalentTo(user);
    }

    [Test]
    public async Task GuidEndpointType()
    {
        var response = await _client.GetAsync("guid");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        //Should not throw
        var guidStr = await response.Content.ReadFromJsonAsync<Guid>();
    }
}