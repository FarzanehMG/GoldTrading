using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using GoldTrading.Application.Features.Orders.Commands;
using GoldTrading.Application.Features.Orders.Response;
using Microsoft.AspNetCore.Mvc.Testing;

namespace GoldTrading.Api.Tests;

public class OrdersApiTests(WebApplicationFactory<Program> factory)
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client = factory.CreateClient();

    private static PlaceBuyOrderCommand ValidCommand(decimal grams = 2.5m) =>
        new(Guid.NewGuid(), grams, 3_500_000m, 50_000m, ["پس‌انداز_بلندمدت"], "note");

    [Fact]
    public async Task PostBuy_WithValidRequest_Returns201_AndOrderIsRetrievable()
    {
        var response = await _client.PostAsJsonAsync("/api/orders/buy", ValidCommand());

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var body = await response.Content.ReadFromJsonAsync<PlaceBuyOrderResult>();
        body!.TotalAmount.Should().Be(8_800_000m);
        body.Status.Should().Be("Settled");

        var get = await _client.GetAsync(response.Headers.Location);
        get.StatusCode.Should().Be(HttpStatusCode.OK);
        var order = await get.Content.ReadFromJsonAsync<OrderResponse>();
        order!.Tags.Should().Contain("پس‌انداز_بلندمدت");
    }

    [Theory]
    [InlineData("0")]
    [InlineData("-1")]
    [InlineData("1.2345")]
    public async Task PostBuy_WithInvalidWeight_Returns400(string grams)
    {
        var command = ValidCommand(decimal.Parse(grams, System.Globalization.CultureInfo.InvariantCulture));

        var response = await _client.PostAsJsonAsync("/api/orders/buy", command);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GetById_WhenMissing_Returns404()
    {
        var response = await _client.GetAsync($"/api/orders/{Guid.NewGuid()}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}