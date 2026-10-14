using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldTrading.Application.Features.Orders.Commands;

public sealed record PlaceBuyOrderCommand(
    Guid UserId,
    decimal WeightInGrams,
    decimal UnitPrice,
    decimal Fee,
    IReadOnlyCollection<string>? Tags,
    string? Note);
