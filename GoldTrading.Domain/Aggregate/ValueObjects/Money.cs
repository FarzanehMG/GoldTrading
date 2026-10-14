using GoldTrading.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldTrading.Domain.Aggregate.ValueObjects;

public sealed record Money
{
    public static readonly Money Zero = new(0m);

    public decimal Amount { get; }

    private Money(decimal amount) => Amount = amount;

    public static Money Of(decimal amount)
    {
        if (amount < 0)
            throw new DomainException("Money cannot be negative.");

        return new Money(amount);
    }

    public bool IsPositive => Amount > 0;

    public Money Add(Money other) => new(Amount + other.Amount);

    public Money Multiply(GoldWeight weight) =>
        new(decimal.Round(Amount * weight.Grams, 0, MidpointRounding.AwayFromZero));
}