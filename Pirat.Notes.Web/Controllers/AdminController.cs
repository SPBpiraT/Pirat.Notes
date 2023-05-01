using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pirat.Notes.Domain.Contracts.Interfaces;
using Pirat.Notes.Domain.Contracts.Models;
using Pirat.Notes.Domain.Contracts.Models.Notes;
using Pirat.Notes.Domain.Contracts.Models.Users;
using Pirat.Notes.Shared.Authorization;

namespace Pirat.Notes.Web.Controllers
{
    [UserAuthorize]
    [AdminAuthorize]
    [ApiController]
    [Route("[controller]")]
    public class AdminController : BaseController
    {
        private readonly IAdminService _adminService;

        public AdminController(ILogger<AdminController> logger,
            IHttpContextAccessor httpContextAccessor,
            IAdminService adminService) : base(httpContextAccessor, logger)
        {
            _adminService = adminService;
        }

        [HttpGet("check-status")]
        public ActionResult<string> GetHi()
        {
            return Ok("hi");
        }

        [HttpDelete("deleteuser/{id}")]
        public IActionResult DeleteUser(int id)
        {
            _adminService.DeleteUser(id);

            _logger.LogInformation("Admin: {AdminName}, Id: {AdminId} deleted user ID: {UserId}", UserName, UserId, id);

            return Ok(new { message = "User deleted successfully!" });
        }

        [HttpDelete("deletenote/{id}")]
        public IActionResult DeleteNote(int id)
        {
            _adminService.DeleteNote(id);

            _logger.LogInformation($"Admin: {UserName}, Id: {UserId} deleted note id: {id}.");

            return Ok(new { message = "Note deleted successfully!" });
        }

        [HttpPut("setuserrole/{id}")]
        public ActionResult<UserModel> SetUserRole(int id, UserRoleUpdateRequest request)
        {
            var responce = _adminService.SetUserRole(id, request);

            _logger.LogInformation(
                $"Admin: {UserName}, Id: {UserId} set new user role for userid: {id}, new userrole: {request.UserRole}.");

            return Ok(responce);
        }

        [HttpPut("updatenote/{id}")]
        public ActionResult<NoteModel> UpdateNote(int id, NoteUpdateRequest model)
        {
            var responce = _adminService.UpdateNote(id, model);

            _logger.LogInformation($"Admin: {UserName}, Id: {UserId} updated note noteId: {id}.");

            return Ok(responce);
        }

        [HttpPut("updateuser/{id}")]
        public ActionResult<UserModel> UpdateUser(int id, UpdateRequest model)
        {
            var responce = _adminService.UpdateUser(id, model);

            _logger.LogInformation($"Admin: {UserName}, Id: {UserId} updated user userId: {id}.");

            return Ok(responce);
        }
    }
}