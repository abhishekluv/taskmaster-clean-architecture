using MediatR;

namespace TaskMaster.Application.Projects.Commands.UpdateProject
{
    public record UpdateProjectCommand(Guid Id, string Name, string Description) : IRequest<Unit>;
}
