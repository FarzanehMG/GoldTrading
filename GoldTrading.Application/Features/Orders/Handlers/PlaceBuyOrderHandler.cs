using GoldTrading.Application.Abstractions;
using GoldTrading.Application.Features.Orders.Commands;
using GoldTrading.Application.Features.Orders.Response;
using GoldTrading.Domain.Aggregate.Entities;
using GoldTrading.Domain.Aggregate.ValueObjects;

namespace GoldTrading.Application.Features.Orders.Handlers;

public sealed class PlaceBuyOrderHandler(
    IOrderRepository orders,
    IWalletRepository wallets,
    IClock clock)
{
    public async Task<PlaceBuyOrderResult> HandleAsync(
        PlaceBuyOrderCommand command, CancellationToken ct = default)
    {
        var now = clock.UtcNow;

        var order = GoldOrder.PlaceBuy(
            command.UserId,
            GoldWeight.FromGrams(command.WeightInGrams),
            Money.Of(command.UnitPrice),
            Money.Of(command.Fee),
            now);

        order.ReplaceTags((command.Tags ?? []).Select(Tag.Create));
        order.UpdateNote(command.Note);

        var balance = await wallets.UpdateAsync(command.UserId, wallet =>
        {
            wallet.Credit(order.Weight);
            return wallet.Balance;
        }, ct);

        order.Settle(now);
        await orders.AddAsync(order, ct);

        return new PlaceBuyOrderResult(
            order.Id, order.Status.ToString(), order.TotalAmount.Amount, balance.Grams);
    }
}
