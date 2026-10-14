using GoldTrading.Application.Abstractions;

namespace GoldTrading.Infrastructure.Common;

public sealed class SystemClock : IClock
{
    public DateTime UtcNow => DateTime.UtcNow;
}
