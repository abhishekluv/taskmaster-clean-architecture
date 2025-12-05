using MediatR;

namespace TaskMaster.Application.Projects.Commands.DeleteProject
{
    public record DeleteProjectCommand(Guid Id) : IRequest<Unit>;
}
