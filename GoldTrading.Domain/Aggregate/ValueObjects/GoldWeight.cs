using GoldTrading.Domain.Common;

namespace GoldTrading.Domain.Aggregate.ValueObjects;

public sealed record GoldWeight
{
    public const int MaxDecimalPlaces = 3;
    public static readonly GoldWeight Zero = new(0m);

    public decimal Grams { get; }

    private GoldWeight(decimal grams) => Grams = grams;

    public bool IsPositive => Grams > 0;

    public static GoldWeight FromGrams(decimal grams)
    {
        if (grams < 0)
            throw new DomainException("Gold weight cannot be negative.");

        if (decimal.Round(grams, MaxDecimalPlaces) != grams)
            throw new DomainException($"Gold weight supports at most {MaxDecimalPlaces} decimal places.");

        return new GoldWeight(grams);
    }

    public GoldWeight Add(GoldWeight other) => new(Grams + other.Grams);

    public GoldWeight Subtract(GoldWeight other)
    {
        if (other.Grams > Grams)
            throw new DomainException("Insufficient gold weight.");
        return new(Grams - other.Grams);
    }

    public bool IsGreaterThan(GoldWeight other) => Grams > other.Grams;
}
