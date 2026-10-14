using GoldTrading.Domain.Aggregate.ValueObjects;
using GoldTrading.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldTrading.Domain.Aggregate.Events;

public sealed record GoldPurchasedDomainEvent(
    Guid OrderId,
    Guid UserId,
    GoldWeight Weight,
    Money TotalAmount,
    DateTime OccurredAtUtc) : IDomainEvent;