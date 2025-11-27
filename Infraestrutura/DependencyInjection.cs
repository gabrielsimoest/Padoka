using Microsoft.AspNetCore.Mvc.Razor;

namespace MedScale.Sys.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddFeatures(this IServiceCollection services)
        {
            //services.AddScoped<Service>();
                
            return services;
        }
    }
}