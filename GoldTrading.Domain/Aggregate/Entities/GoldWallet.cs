using GoldTrading.Domain.Aggregate.ValueObjects;
using GoldTrading.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldTrading.Domain.Aggregate.Entities;

public sealed class GoldWallet
{
    public Guid Id { get; }
    public Guid UserId { get; }
    public GoldWeight Balance { get; private set; }

    private GoldWallet(Guid id, Guid userId)
    {
        Id = id;
        UserId = userId;
        Balance = GoldWeight.Zero;
    }

    public static GoldWallet Open(Guid userId)
    {
        if (userId == Guid.Empty)
            throw new DomainException("UserId is required.");

        return new GoldWallet(Guid.NewGuid(), userId);
    }

    public void Credit(GoldWeight weight)
    {
        if (!weight.IsPositive)
            throw new DomainException("Credit weight must be greater than zero.");

        Balance = Balance.Add(weight);
    }
}