using MediatR;

namespace TaskMaster.Application.Projects.Commands.CreateProject
{
    public record CreateProjectCommand(string Name, string? Description, string OwnerUserId) : IRequest<Guid>;
}
