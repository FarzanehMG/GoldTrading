using GoldTrading.Application.Features.Orders.Handlers;
using Microsoft.Extensions.DependencyInjection;

namespace GoldTrading.Application.DI;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<PlaceBuyOrderHandler>();
        return services;
    }
}
