using Microsoft.AspNetCore.Mvc.Razor;
using Padoka.Services;

namespace MedScale.Sys.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddFeatures(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ICardapioService, CardapioService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ICardapioService, CardapioService>();
            services.AddScoped<IPedidoService, PedidoService>();
            services.AddScoped<IAdminCardapioService, AdminCardapioService>();

            return services;
        }
    }
}