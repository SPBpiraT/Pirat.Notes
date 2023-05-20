using Microsoft.Extensions.DependencyInjection;
using Pirat.Notes.Domain.Contracts.Interfaces;
using Pirat.Notes.Domain.Implementations.Services;

namespace Pirat.Notes.Domain.Implementations.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<INoteService, NoteService>();
            services.AddScoped<IAdminService, AdminService>();

            return services;
        }
    }
}