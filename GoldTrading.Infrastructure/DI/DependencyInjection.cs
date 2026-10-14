using GoldTrading.Application.Abstractions;
using GoldTrading.Infrastructure.Common;
using GoldTrading.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldTrading.Infrastructure.DI;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IOrderRepository, InMemoryOrderRepository>();
        services.AddSingleton<IWalletRepository, InMemoryWalletRepository>();
        services.AddSingleton<IClock, SystemClock>();
        return services;
    }
}