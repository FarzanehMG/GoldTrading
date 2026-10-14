using GoldTrading.Domain.Common;

namespace GoldTrading.Domain.Aggregate.ValueObjects;

public sealed record Tag
{
    public const int MaxLength = 50;

    public string Value { get; }

    private Tag(string value) => Value = value;

    public static Tag Create(string? value)
    {
        var trimmed = value?.Trim();

        if (string.IsNullOrWhiteSpace(trimmed))
            throw new DomainException("Tag cannot be empty.");

        return trimmed.Length > MaxLength ? throw new DomainException($"Tag cannot exceed {MaxLength} characters.") : new Tag(trimmed);
    }
}
