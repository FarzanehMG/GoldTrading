namespace GoldTrading.Domain.Common;

public interface IDomainEvent
{
    DateTime OccurredAtUtc { get; }
}
