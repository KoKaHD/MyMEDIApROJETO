using Microsoft.Extensions.DependencyInjection;
using RCLAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCLAPI.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddRCLApiServices(this IServiceCollection services, string apiBaseUrl)
        {
            services.AddScoped<TokenService>();
            services.AddScoped<ApiService>();

            services.AddHttpClient("Api", client =>
            {
                client.BaseAddress = new Uri(apiBaseUrl);
            })
            .AddHttpMessageHandler<AuthHandler>();

            services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>()
                                       .CreateClient("Api"));

            services.AddScoped<AuthHandler>();

            return services;
        }
    }
}
