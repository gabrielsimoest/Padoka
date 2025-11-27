using Microsoft.AspNetCore.Mvc.Razor;
using Padoka.Services;

namespace MedScale.Sys.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddFeatures(this IServiceCollection services)
        {
            // Auth Service
            services.AddScoped<IAuthService, AuthService>();

            // Cardapio Service
            services.AddScoped<ICardapioService, CardapioService>();

            return services;
        }
    }
}