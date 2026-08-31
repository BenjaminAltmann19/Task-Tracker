using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using Xunit;

namespace TaskTrackerApi.Tests;

public class TaskApiTests
{
    [Fact]
    public async Task RootEndpoint_ReturnsSuccess()
    {
        await using var application =
            new WebApplicationFactory<Program>();

        var client = application.CreateClient();

        var response = await client.GetAsync("/");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}