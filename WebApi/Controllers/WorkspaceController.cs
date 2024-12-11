using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebApi.Controllers
{
    /// <summary>
    /// Kontroler obsługujący zarządzanie przestrzeniami roboczymi (workspaces).
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class WorkspaceController : ControllerBase
    {
        private readonly IWorkspaceService _workspaceService;

        public WorkspaceController(IWorkspaceService workspaceService)
        {
            _workspaceService = workspaceService;
        }

        /// <summary>
        /// Pobiera wszystkie przestrzenie robocze przypisane do aktualnie zalogowanego użytkownika.
        /// </summary>
        /// <returns>Lista przestrzeni roboczych użytkownika.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WorkspaceDto>>> GetWorkspaces()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var workspaces = await _workspaceService.GetAllWorkspacesByUserIdAsync(userId);
            if(workspaces == null)
            {
                return NotFound("no workspaces");
            }
            return Ok(workspaces);
        }

        /// <summary>
        /// Pobiera szczegóły określonej przestrzeni roboczej na podstawie jej identyfikatora.
        /// </summary>
        /// <param name="id">Identyfikator przestrzeni roboczej.</param>
        /// <returns>Szczegóły przestrzeni roboczej.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<WorkspaceDto>> GetWorkspace(Guid id)
        {
            var workspace = await _workspaceService.GetWorkspaceByIdAsync(id);
            if (workspace == null)
                return NotFound();

            return Ok(workspace);
        }

        /// <summary>
        /// Tworzy nową przestrzeń roboczą.
        /// </summary>
        /// <param name="workspaceDto">Dane przestrzeni roboczej.</param>
        /// <returns>Utworzona przestrzeń robocza.</returns>
        [HttpPost]
        public async Task<ActionResult<WorkspaceDto>> CreateWorkspace([FromBody] WorkspaceDto workspaceDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();



            var createdWorkspace = await _workspaceService.CreateWorkspaceAsync(workspaceDto, userId);
            return CreatedAtAction(nameof(GetWorkspace), new { id = createdWorkspace.Id }, createdWorkspace);
        }

        /// <summary>
        /// Aktualizuje istniejącą przestrzeń roboczą.
        /// </summary>
        /// <param name="id">Identyfikator przestrzeni roboczej.</param>
        /// <param name="workspaceDto">Zaktualizowane dane przestrzeni roboczej.</param>
        /// <returns>Odpowiedź HTTP wskazująca status operacji.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateWorkspace(Guid id, [FromBody] WorkspaceDto workspaceDto)
        {
            if (id != workspaceDto.Id)
                return BadRequest("ID mismatch");

            var existingWorkspace = await _workspaceService.GetWorkspaceByIdAsync(id);
            if (existingWorkspace == null)
                return NotFound();


            var updatedWorkspace = await _workspaceService.UpdateWorkspaceAsync(workspaceDto);
            if (updatedWorkspace == null)
                return NotFound();

            return NoContent();
        }

        /// <summary>
        /// Usuwa przestrzeń roboczą na podstawie jej identyfikatora.
        /// </summary>
        /// <param name="id">Identyfikator przestrzeni roboczej.</param>
        /// <returns>Odpowiedź HTTP wskazująca status operacji.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWorkspace(Guid id)
        {
            var existingWorkspace = await _workspaceService.GetWorkspaceByIdAsync(id);
            if (existingWorkspace == null)
                return NotFound();


            await _workspaceService.DeleteWorkspaceAsync(id);
            return NoContent();
        }
    }
}