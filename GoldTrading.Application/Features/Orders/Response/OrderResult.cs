namespace GoldTrading.Application.Features.Orders.Response;

public sealed record OrderResponse(
    Guid OrderId,
    Guid UserId,
    string Type,
    string Status,
    decimal WeightInGrams,
    decimal UnitPrice,
    decimal Fee,
    decimal TotalAmount,
    IReadOnlyCollection<string> Tags,
    string? Note,
    DateTime CreatedAtUtc);
