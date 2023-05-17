using Microsoft.Extensions.DependencyInjection;

using Pirat.Notes.DAL.Contracts;

namespace Pirat.Notes.DAL.Implementations.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<INoteRepository, NoteRepository>();

            return services;
        }
    }
}