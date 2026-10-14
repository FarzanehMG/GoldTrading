using GoldTrading.Domain.Aggregate.ValueObjects;
using GoldTrading.Domain.Common;

namespace GoldTrading.Domain.Aggregate.Entities;

public sealed class GoldWallet
{
    #region Properties

    public Guid Id { get; }
    public Guid UserId { get; }
    public GoldWeight Balance { get; private set; }

    #endregion

    #region Constructors

    private GoldWallet(Guid id, Guid userId)
    {
        Id = id;
        UserId = userId;
        Balance = GoldWeight.Zero;
    }

    #endregion

    #region Methods

    public static GoldWallet Open(Guid userId)
    {
        return userId == Guid.Empty ? 
            throw new DomainException("UserId is required.") : 
            new GoldWallet(Guid.NewGuid(), userId);
    }

    public void Credit(GoldWeight weight)
    {
        if (!weight.IsPositive)
            throw new DomainException("Credit weight must be greater than zero.");

        Balance = Balance.Add(weight);
    }

    #endregion
}