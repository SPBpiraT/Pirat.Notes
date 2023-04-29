using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ILogger = Microsoft.Extensions.Logging.ILogger;


//using Pirat.Notes.DAL.Contracts.Entities;

namespace Pirat.Notes.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public abstract class BaseController : Controller
    {
        protected ILogger _logger;

        public BaseController(IHttpContextAccessor httpContextAccessor, ILogger logger)
        {
            if (httpContextAccessor.HttpContext.Items["Id"] != null)
                UserId = (int)httpContextAccessor.HttpContext.Items["Id"];

            if (httpContextAccessor.HttpContext.Items["Username"] != null)
                UserName = httpContextAccessor.HttpContext.Items["Username"].ToString();

            if (httpContextAccessor.HttpContext.Items["Role"] != null)
                Role = httpContextAccessor.HttpContext.Items["Role"].ToString();

            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected int UserId { get; set; }
        protected string UserName { get; set; }
        protected string Role { get; set; }
    }
}