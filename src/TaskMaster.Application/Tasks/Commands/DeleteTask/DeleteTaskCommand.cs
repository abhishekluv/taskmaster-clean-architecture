using MediatR;

namespace TaskMaster.Application.Tasks.Commands.DeleteTask
{
    public record DeleteTaskCommand(Guid Id) : IRequest<Unit>;
}
