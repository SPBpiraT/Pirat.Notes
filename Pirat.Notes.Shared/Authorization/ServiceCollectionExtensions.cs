using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Pirat.Notes.Shared.Helpers;

namespace Pirat.Notes.Shared.Authorization
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAuthServices(this IServiceCollection services)
        {
            services
                .AddOptions<AuthSettings>()
                .Configure<IConfiguration>(
                    (options, config) => config.GetSection("AppSettings").Bind(options));

            services.AddScoped<IJwtUtils, JwtUtils>();

            return services;
        }
    }
}