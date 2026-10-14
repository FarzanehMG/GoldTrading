using FluentAssertions;
using GoldTrading.Domain.Aggregate.Entities;
using GoldTrading.Domain.Aggregate.Enums;
using GoldTrading.Domain.Aggregate.ValueObjects;
using GoldTrading.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldTrading.Domain.Tests;

public class GoldOrderTests
{
    private static readonly DateTime Now = new(2026, 10, 14, 8, 0, 0, DateTimeKind.Utc);

    private static GoldOrder CreateValidOrder(
        decimal grams = 2.5m, decimal unitPrice = 3_500_000m, decimal fee = 50_000m) =>
        GoldOrder.PlaceBuy(
            Guid.NewGuid(),
            GoldWeight.FromGrams(grams),
            Money.Of(unitPrice),
            Money.Of(fee),
            Now);

    [Fact]
    public void PlaceBuy_CalculatesTotalAmount_AsWeightTimesPricePlusFee()
    {
        var order = CreateValidOrder(grams: 2.5m, unitPrice: 3_500_000m, fee: 50_000m);

        order.TotalAmount.Amount.Should().Be(8_800_000m); 
    }

    [Fact]
    public void PlaceBuy_StartsAsPendingBuyOrder()
    {
        var order = CreateValidOrder();

        order.Status.Should().Be(OrderStatus.Pending);
        order.Type.Should().Be(OrderType.Buy);
    }

    [Fact]
    public void PlaceBuy_RoundsOnlyOnce_AwayFromZero()
    {
        var order = CreateValidOrder(grams: 0.333m, unitPrice: 1_001m, fee: 10m);

        order.TotalAmount.Amount.Should().Be(343m);
    }

    [Fact]
    public void PlaceBuy_WithZeroWeight_Throws()
    {
        var act = () => CreateValidOrder(grams: 0m);

        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void PlaceBuy_WithZeroUnitPrice_Throws()
    {
        var act = () => CreateValidOrder(unitPrice: 0m);

        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void Settle_ChangesStatusToSettled()
    {
        var order = CreateValidOrder();

        order.Settle();

        order.Status.Should().Be(OrderStatus.Settled);
    }

    [Fact]
    public void Settle_WhenAlreadySettled_Throws()
    {
        var order = CreateValidOrder();
        order.Settle();

        var act = () => order.Settle();

        act.Should().Throw<DomainException>();
    }
}