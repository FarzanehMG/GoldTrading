using GoldTrading.Domain.Aggregate.ValueObjects;
using GoldTrading.Domain.Common;

namespace GoldTrading.Domain.Aggregate.Events;

public sealed record GoldPurchasedDomainEvent(
    Guid OrderId,
    Guid UserId,
    GoldWeight Weight,
    Money TotalAmount,
    DateTime OccurredAtUtc) : IDomainEvent;