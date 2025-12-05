using MediatR;

namespace TaskMaster.Application.Tasks.Commands.ChangeTaskStatus
{
    public record ChangeTaskStatusCommand(Guid Id, string Status) : IRequest<Unit>;
}
