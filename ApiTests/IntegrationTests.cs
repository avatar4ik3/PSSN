using System.Net;
using System.Text.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using PSSN.Common;

namespace PSSN.ApiTests;

public class IntegrationTests
{

    private readonly HttpClient _client;
    private readonly WebApplicationFactory<Api.Program> _factory;
    public IntegrationTests()
    {
        _factory = new WebApplicationFactory<Api.Program>();
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task App_StatusCode_ShouldBe_Ok()
    {
        var response = await _client.GetAsync("/api/v1/test");

        using var sr = new StreamReader(await response.Content.ReadAsStreamAsync());
        var src = await sr.ReadToEndAsync();
        var data = JsonSerializer.Deserialize<IEnumerable<VectorResponse>>(src);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        data.Count().Should().Be(2001);
    }

}