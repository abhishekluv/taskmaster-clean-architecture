using MediatR;

namespace TaskMaster.Application.Tasks.Commands.AssignTagToTask
{
    public record AssignTagToTaskCommand(
        Guid TaskId,
        string TagName,
        string? Color) : IRequest<Unit>;
}
