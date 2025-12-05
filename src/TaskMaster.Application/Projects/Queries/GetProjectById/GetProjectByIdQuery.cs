using MediatR;

using TaskMaster.Application.Projects.Dtos;

namespace TaskMaster.Application.Projects.Queries.GetProjectById
{
    public record GetProjectByIdQuery(Guid Id) : IRequest<ProjectDto?>;
}
