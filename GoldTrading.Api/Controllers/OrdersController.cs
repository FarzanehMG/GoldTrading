using GoldTrading.Application.Features.Orders.Commands;
using GoldTrading.Application.Features.Orders.Handlers;
using GoldTrading.Application.Features.Orders.Queries;
using GoldTrading.Application.Features.Orders.Response;
using Microsoft.AspNetCore.Mvc;

namespace GoldTrading.Api.Controllers;

[ApiController]
[Route("api/orders")]
public sealed class OrdersController(
    PlaceBuyOrderHandler placeBuyOrder,
    GetOrderByIdHandler getOrderById) : ControllerBase
{
    [HttpPost("buy")]
    [ProducesResponseType(typeof(PlaceBuyOrderResult), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> PlaceBuy(PlaceBuyOrderCommand command, CancellationToken ct)
    {
        var result = await placeBuyOrder.HandleAsync(command, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.OrderId }, result);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(OrderResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var order = await getOrderById.HandleAsync(new GetOrderByIdQuery(id), ct);
        return order is null ? NotFound() : Ok(order);
    }
}
