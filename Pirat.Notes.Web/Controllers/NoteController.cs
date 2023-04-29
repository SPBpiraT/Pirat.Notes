using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pirat.Notes.Domain.Contracts.Interfaces;
using Pirat.Notes.Domain.Contracts.Models;
using Pirat.Notes.Shared.Authorization;

namespace Pirat.Notes.Web.Controllers
{
    [UserAuthorize]
    [ApiController]
    [Route("[controller]")]
    public class NoteController : BaseController
    {
        private readonly INoteService _noteService;

        public NoteController(ILogger<NoteController> logger,
            IHttpContextAccessor httpContextAccessor,
            INoteService noteService) : base(httpContextAccessor, logger)
        {
            _noteService = noteService;
        }

        [AllowAnonymous]
        [HttpGet("check-status")]
        public IActionResult CheckStatus()
        {
            return Ok("Active");
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public ActionResult<NoteModel> GetById(int id)
        {
            var model = _noteService.GetById(id);

            if (model == null) return NotFound("Note not found");

            return Ok(model);
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult<List<NoteModel>> GetAll()
        {
            var collection = _noteService.GetAll();

            if (collection == null || !collection.Any()) return collection;

            return Ok(collection);
        }

        [HttpPost("create")]
        public IActionResult Create(NoteModel model)
        {
            if (model.Note.Length < 1)
                return BadRequest("Note must contain at least one symbol.");

            model.UserName = UserName;

            model.UserId = UserId;

            _noteService.CreateNote(model);

            _logger.LogInformation($"User {UserName} Id {UserId} added new note.");

            return Ok(new { message = "New note was created." });
        }

        [HttpDelete("deletenote/{id}")]
        public IActionResult Delete(int id)
        {
            if (UserId == _noteService.GetById(id).UserId)
            {
                _noteService.Delete(id);

                _logger.LogInformation($"User {UserName} Id {UserId} deleted note id {id}.");

                return Ok(new { message = "Note was deleted!" });
            }

            return new StatusCodeResult(403);
        }
    }
}