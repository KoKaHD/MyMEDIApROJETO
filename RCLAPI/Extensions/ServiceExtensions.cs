using Microsoft.Extensions.DependencyInjection;
using RCLAPI.Services;

namespace RCLAPI.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddRCLApiServices(this IServiceCollection services, string apiBaseUrl)
        {
            services.AddScoped<TokenService>();
            services.AddScoped<AuthHandler>();

            // Client público (sem JWT)
            services.AddHttpClient("PublicApi", client =>
            {
                client.BaseAddress = new Uri(apiBaseUrl);
            });

            // Client autenticado (com JWT)
            services.AddHttpClient("AuthApi", client =>
            {
                client.BaseAddress = new Uri(apiBaseUrl);
            })
            .AddHttpMessageHandler<AuthHandler>();

            services.AddScoped<ApiService>();

            return services;
        }
    }
}