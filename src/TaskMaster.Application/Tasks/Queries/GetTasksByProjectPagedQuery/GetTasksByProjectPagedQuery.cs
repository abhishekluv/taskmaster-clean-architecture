using MediatR;

using TaskMaster.Application.Common.Models;
using TaskMaster.Application.Tasks.Dtos;

namespace TaskMaster.Application.Tasks.Queries.GetTasksByProjectPagedQuery
{
    public record GetTasksByProjectPagedQuery(
        Guid ProjectId,
        int PageNumber = 1,
        int PageSize = 10) : IRequest<PagedResult<TaskDto>>;
}
