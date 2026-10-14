using GoldTrading.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        if (trimmed.Length > MaxLength)
            throw new DomainException($"Tag cannot exceed {MaxLength} characters.");

        return new Tag(trimmed);
    }
}
