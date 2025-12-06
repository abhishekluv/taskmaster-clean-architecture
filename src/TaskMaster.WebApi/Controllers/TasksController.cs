using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TaskMaster.Application.Common.Models;
using TaskMaster.Application.Reminders.Commands.CreateReminder;
using TaskMaster.Application.Tasks.Commands.AssignTagToTask;
using TaskMaster.Application.Tasks.Commands.ChangeTaskStatus;
using TaskMaster.Application.Tasks.Commands.CreateTask;
using TaskMaster.Application.Tasks.Commands.DeleteTask;
using TaskMaster.Application.Tasks.Commands.RemoveTagFromTask;
using TaskMaster.Application.Tasks.Commands.UpdateTask;
using TaskMaster.Application.Tasks.Dtos;
using TaskMaster.Application.Tasks.Queries.GetTaskById;
using TaskMaster.Application.Tasks.Queries.GetTasksByProjectPagedQuery;
using TaskMaster.WebApi.Dtos.Tasks;

namespace TaskMaster.WebApi.Controllers
{
    //nested routes
    [Route("api/projects/{projectId:guid}/tasks")]
    [ApiController]
    [Authorize]
    public class TasksController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TasksController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("{id:guid}/tags")]
        public async Task<IActionResult> AssignTag(
            Guid projectId,
            Guid id,
            [FromBody] AssignTagToTaskRequestDto request,
            CancellationToken ct)
        {
            // NOTE: We assume the task belongs to the given project; if you want,
            // we can enforce this inside the handler later by checking ProjectId.
            var command = new AssignTagToTaskCommand(
                id,
                request.TagName,
                request.Color);

            await _mediator.Send(command, ct);
            return NoContent();
        }

        [HttpDelete("{id:guid}/tags")]
        public async Task<IActionResult> RemoveTag(
            Guid projectId,
            Guid id,
            [FromBody] RemoveTagFromTaskRequestDto request,
            CancellationToken ct)
        {
            var command = new RemoveTagFromTaskCommand(
                id,
                request.TagName);

            await _mediator.Send(command, ct);
            return NoContent();
        }

        [HttpPost("{id:guid}/reminders")]
        public async Task<ActionResult<Guid>> CreateReminder(
            Guid projectId,
            Guid id,
            [FromBody] CreateReminderRequestDto request,
            CancellationToken ct)
        {
            var command = new CreateReminderCommand(
                id,
                request.ReminderTime);

            var reminderId = await _mediator.Send(command, ct);
            return CreatedAtAction(nameof(GetById), new { projectId, id }, reminderId);
        }


        [HttpPost]
        public async Task<ActionResult<Guid>> Create(
        Guid projectId,
        [FromBody] CreateTaskRequestDto request,
        CancellationToken ct)
        {
            var userId = GetUserId();

            var command = new CreateTaskCommand(
                projectId,
                request.Title,
                request.Description,
                request.DueDate,
                request.Priority,
                userId!);

            var id = await _mediator.Send(command, ct);
            return CreatedAtAction(nameof(GetById), new { projectId, id }, id);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<TaskDto>> GetById(Guid projectId, Guid id, CancellationToken ct)
        {
            var result = await _mediator.Send(new GetTaskByIdQuery(id), ct);
            return result is null ? NotFound() : Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<TaskDto>>> GetTasks(
            Guid projectId,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            CancellationToken ct = default)
        {
            var result = await _mediator.Send(
                new GetTasksByProjectPagedQuery(projectId, pageNumber, pageSize),
                ct);

            return Ok(result);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(
            Guid projectId,
            Guid id,
            [FromBody] UpdateTaskRequestDto request,
            CancellationToken ct)
        {
            var command = new UpdateTaskCommand(id, request.Title, request.Description, request.DueDate, request.Priority);
            await _mediator.Send(command, ct);
            return NoContent();
        }

        [HttpPatch("{id:guid}/status")]
        public async Task<IActionResult> ChangeStatus(
            Guid projectId,
            Guid id,
            [FromBody] ChangeTaskStatusRequestDto request,
            CancellationToken ct)
        {
            await _mediator.Send(new ChangeTaskStatusCommand(id, request.Status), ct);
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid projectId, Guid id, CancellationToken ct)
        {
            await _mediator.Send(new DeleteTaskCommand(id), ct);
            return NoContent();
        }

        private string? GetUserId()
        {
            return User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
                ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
