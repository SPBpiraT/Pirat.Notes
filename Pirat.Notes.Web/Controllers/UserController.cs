using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pirat.Notes.Domain.Contracts.Interfaces;
using Pirat.Notes.Domain.Contracts.Models;
using Pirat.Notes.Domain.Contracts.Models.Users;
using Pirat.Notes.Shared.Authorization;

namespace Pirat.Notes.Web.Controllers
{
    [UserAuthorize]
    [ApiController]
    [Route("[controller]")]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;

        public UserController(ILogger<UserController> logger,
            IHttpContextAccessor httpContextAccessor,
            IUserService userService) : base(httpContextAccessor, logger)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate(AuthenticateRequest model)
        {
            var response = _userService.Authenticate(model);

            _logger.LogInformation($"User: {response.Username}, id: {response.Id} authenticated.");

            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register(RegisterRequest model)
        {
            _userService.Register(model);

            _logger.LogInformation($"New user registered. Username: {model.Username}");

            return Ok(new { message = "Registration successful" });
        }

        [HttpGet]
        public ActionResult<List<UserModel>> GetAll() // pagination?
        {
            var users = _userService.GetAll();

            return Ok(users);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var user = _userService.GetById(id);

            return Ok(user);
        }

        [HttpPut("updateuser/{id}")]
        public IActionResult Update(int id, UpdateRequest model)
        {
            if (UserId == id)
            {
                _userService.Update(id, model);

                _logger.LogInformation($"User: {UserName}, id: {UserId} updated account");

                return Ok(new { message = "User updated successfully!" });
            }

            return new StatusCodeResult(403);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            // fail fast and early, avoid unnecessary nesting
            if (UserId != id)
                return new StatusCodeResult(403);

            _userService.Delete(id);

            _logger.LogInformation($"User: {UserName}, id: {UserId} delete his account");

            return Ok(new { message = "User deleted successfully!" });
        }
    }
}