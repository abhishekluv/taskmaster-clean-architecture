using MediatR;

namespace TaskMaster.Application.Tasks.Commands.CreateTask
{
    public record CreateTaskCommand(
        Guid ProjectId,
        string Title,
        string? Description,
        DateTime? DueDate,
        string Priority,
        string UserId) : IRequest<Guid>;
}
