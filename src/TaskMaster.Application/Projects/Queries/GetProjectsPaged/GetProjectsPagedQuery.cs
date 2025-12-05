using MediatR;

using TaskMaster.Application.Common.Models;
using TaskMaster.Application.Projects.Dtos;

namespace TaskMaster.Application.Projects.Queries.GetProjectsPaged
{
    public record GetProjectsPagedQuery(
        int PageNumber = 1,
        int PageSize = 10,
        string? Search = null,
        string? OwnerUserId = null) : IRequest<PagedResult<ProjectDto>>;
}
