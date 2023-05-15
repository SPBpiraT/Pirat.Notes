using Microsoft.AspNetCore.Builder;

namespace Pirat.Notes.Shared.Authorization
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseJwtAuth(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<JwtMiddleware>();

            return builder;
        }
    }
}