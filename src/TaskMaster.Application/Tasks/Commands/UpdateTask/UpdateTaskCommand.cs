using MediatR;

namespace TaskMaster.Application.Tasks.Commands.UpdateTask
{
    public record UpdateTaskCommand(
        Guid Id,
        string Title,
        string? Description,
        DateTime? DueDate,
        string Priority) : IRequest<Unit>;
}
