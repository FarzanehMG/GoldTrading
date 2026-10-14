using FluentValidation;
using GoldTrading.Application.Features.Orders.Handlers;
using GoldTrading.Application.Features.Orders.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace GoldTrading.Application.DI;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<PlaceBuyOrderCommandValidator>();
        services.AddScoped<PlaceBuyOrderHandler>();
        services.AddScoped<GetOrderByIdHandler>();
        return services;
    }
}
