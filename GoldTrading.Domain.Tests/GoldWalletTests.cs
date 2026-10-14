using GoldTrading.Domain.Aggregate.Entities;
using GoldTrading.Domain.Aggregate.ValueObjects;
using GoldTrading.Domain.Common;
using FluentAssertions;

namespace GoldTrading.Domain.Tests;

public class GoldWalletTests
{
    [Fact]
    public void Open_StartsWithZeroBalance()
    {
        var wallet = GoldWallet.Open(Guid.NewGuid());

        wallet.Balance.Should().Be(GoldWeight.Zero);
    }

    [Fact]
    public void Open_WithEmptyUserId_Throws()
    {
        var act = () => GoldWallet.Open(Guid.Empty);

        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void Credit_IncreasesBalance()
    {
        var wallet = GoldWallet.Open(Guid.NewGuid());

        wallet.Credit(GoldWeight.FromGrams(2.5m));
        wallet.Credit(GoldWeight.FromGrams(1.250m));

        wallet.Balance.Grams.Should().Be(3.750m);
    }

    [Fact]
    public void Credit_WithZeroWeight_Throws()
    {
        var wallet = GoldWallet.Open(Guid.NewGuid());

        var act = () => wallet.Credit(GoldWeight.Zero);

        act.Should().Throw<DomainException>();
    }
}
