namespace GoldTrading.Application.Features.Orders.Response;

public sealed record PlaceBuyOrderResult(
    Guid OrderId,
    string Status,
    decimal TotalAmount,
    decimal WalletBalanceInGrams);