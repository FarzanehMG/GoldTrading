using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldTrading.Domain.Common;

public interface IDomainEvent
{
    DateTime OccurredAtUtc { get; }
}
