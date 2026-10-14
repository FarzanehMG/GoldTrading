using GoldTrading.Application.Abstractions;
using GoldTrading.Application.Features.Orders.Commands;
using GoldTrading.Application.Features.Orders.Response;

namespace GoldTrading.Application.Features.Orders.Handlers;

public sealed class GetOrderByIdHandler(IOrderRepository orders)
{
    public async Task<OrderResponse?> HandleAsync(
        GetOrderByIdQuery query, CancellationToken ct = default)
    {
        var order = await orders.GetByIdAsync(query.Id, ct);
        if (order is null) return null;

        return new OrderResponse(
            order.Id,
            order.UserId,
            order.Type.ToString(),
            order.Status.ToString(),
            order.Weight.Grams,
            order.UnitPrice.Amount,
            order.Fee.Amount,
            order.TotalAmount.Amount,
            order.Tags.Select(t => t.Value).ToList(),
            order.UserNote,
            order.CreatedAtUtc);
    }
}
