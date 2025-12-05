using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TaskMaster.Application.Common.Models;
using TaskMaster.Application.Projects.Commands.CreateProject;
using TaskMaster.Application.Projects.Commands.DeleteProject;
using TaskMaster.Application.Projects.Commands.UpdateProject;
using TaskMaster.Application.Projects.Dtos;
using TaskMaster.Application.Projects.Queries.GetProjectById;
using TaskMaster.Application.Projects.Queries.GetProjectsPaged;
using TaskMaster.WebApi.Dtos.Projects;

namespace TaskMaster.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProjectsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProjectsController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpPost]
        public async Task<ActionResult<Guid>> CreateProject([FromBody] CreateProjectRequestDto request, CancellationToken ct)
        {
            var userId = GetUserId();

            if (userId is null) return Unauthorized();

            var command = new CreateProjectCommand(
                request.Name, request.Description, userId);

            var id = await _mediator.Send(command, ct);

            return CreatedAtAction(nameof(GetById), new { id }, id);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ProjectDto>> GetById(Guid id, CancellationToken ct)
        {
            var result = await _mediator.Send(new GetProjectByIdQuery(id), ct);

            if (result is null) return NotFound();

            return Ok(result);
        }



        [HttpGet]
        public async Task<ActionResult<PagedResult<ProjectDto>>> GetPaged(
            [FromQuery] int pageNumber = 1, 
            [FromQuery] int pageSize = 10, 
            [FromQuery] string? search = null, 
            CancellationToken ct = default)
        {
            var userId = GetUserId();

            if (userId == null) return Unauthorized();

            var query = new GetProjectsPagedQuery(pageNumber, pageSize, search, userId);

            var result = await _mediator.Send(query, ct);

            return Ok(result);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProjectRequestDto request, CancellationToken ct)
        {
            var command = new UpdateProjectCommand(id, request.Name, request.Description);
            await _mediator.Send(command, ct);
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
        {
            await _mediator.Send(new DeleteProjectCommand(id), ct);
            return NoContent();
        }

        private string? GetUserId()
        {
            // We stored the user Id in the JWT as 'sub' (subject).
            var sub = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

            // Also try standard NameIdentifier in case we add it later.
            var nameId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return sub ?? nameId;
        }
    }
}
