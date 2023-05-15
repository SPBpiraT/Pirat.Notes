using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;


//using Pirat.Notes.DAL.Contracts.Entities;

namespace Pirat.Notes.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public abstract class BaseController : Controller
    {
        protected readonly ILogger _logger;

        protected BaseController(IHttpContextAccessor httpContextAccessor, ILogger logger)
        {
            var httpContextItems = httpContextAccessor.HttpContext?.Items;

            if (httpContextItems is not null)
            {
                UserId = (int)httpContextItems["Id"];
                UserName = httpContextItems["Username"] as string;
                Role = httpContextItems["Role"] as string;
            }

            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected int UserId { get; set; }
        protected string UserName { get; set; }
        protected string Role { get; set; }
    }
}