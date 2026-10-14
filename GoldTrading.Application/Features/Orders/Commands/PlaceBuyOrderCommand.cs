namespace GoldTrading.Application.Features.Orders.Commands;

public sealed record PlaceBuyOrderCommand(
    Guid UserId,
    decimal WeightInGrams,
    decimal UnitPrice,
    decimal Fee,
    IReadOnlyCollection<string>? Tags,
    string? Note);
