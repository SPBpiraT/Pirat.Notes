using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Pirat.Notes.Domain.Contracts.Models;

namespace Pirat.Notes.Shared.Authorization
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AdminAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private readonly string[] allowedRoles = { "Admin", "Moderator" };

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // authorization
            var user = (UserModel)context.HttpContext.Items["User"];

            if (!allowedRoles.Contains(user.UserRole))
                context.Result = new JsonResult(new { message = "Unauthorized" })
                    { StatusCode = StatusCodes.Status401Unauthorized };
        }
    }
}