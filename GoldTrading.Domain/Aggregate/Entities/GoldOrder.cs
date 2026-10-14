using GoldTrading.Domain.Aggregate.Enums;
using GoldTrading.Domain.Aggregate.ValueObjects;
using GoldTrading.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldTrading.Domain.Aggregate.Entities;

public sealed class GoldOrder
{
    public Guid Id { get; }
    public Guid UserId { get; }
    public OrderType Type { get; }
    public GoldWeight Weight { get; }
    public Money UnitPrice { get; }
    public Money Fee { get; }
    public Money TotalAmount { get; }
    public OrderStatus Status { get; private set; }
    public DateTime CreatedAtUtc { get; }

    private GoldOrder(
        Guid id, Guid userId, GoldWeight weight,
        Money unitPrice, Money fee, Money totalAmount, DateTime createdAtUtc)
    {
        Id = id;
        UserId = userId;
        Type = OrderType.Buy;
        Weight = weight;
        UnitPrice = unitPrice;
        Fee = fee;
        TotalAmount = totalAmount;
        Status = OrderStatus.Pending;
        CreatedAtUtc = createdAtUtc;
    }

    public static GoldOrder PlaceBuy(
        Guid userId, GoldWeight weight, Money unitPrice, Money fee, DateTime nowUtc)
    {
        if (userId == Guid.Empty)
            throw new DomainException("UserId is required.");

        if (!weight.IsPositive)
            throw new DomainException("Order weight must be greater than zero.");

        if (!unitPrice.IsPositive)
            throw new DomainException("Unit price must be greater than zero.");

        // TotalAmount = (Weight × UnitPrice) + Fee
        var total = unitPrice.Multiply(weight).Add(fee);

        return new GoldOrder(Guid.NewGuid(), userId, weight, unitPrice, fee, total, nowUtc);
    }

    public void Settle()
    {
        if (Status != OrderStatus.Pending)
            throw new DomainException($"Only pending orders can be settled. Current status: {Status}.");

        Status = OrderStatus.Settled;
    }
}
