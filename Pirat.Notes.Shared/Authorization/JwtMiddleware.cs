using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Pirat.Notes.Domain.Contracts.Interfaces;

namespace Pirat.Notes.Shared.Authorization
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IUserService userService, IJwtUtils jwtUtils)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            var userId = jwtUtils.ValidateToken(token);

            if (userId != null)
            {
                var user = userService.GetById(userId.Value);

                // attach user to context on successful jwt validation
                context.Items["User"] = user;

                context.Items["Id"] = user.Id;

                context.Items["Username"] = user.Username;

                context.Items["Role"] = user.UserRole;
            }

            await _next(context);
        }
    }
}