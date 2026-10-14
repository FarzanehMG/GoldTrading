using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldTrading.Application.Features.Orders.Response;

public sealed record PlaceBuyOrderResult(
    Guid OrderId,
    string Status,
    decimal TotalAmount,
    decimal WalletBalanceInGrams);