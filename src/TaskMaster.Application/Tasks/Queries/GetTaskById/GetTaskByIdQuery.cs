using MediatR;

using TaskMaster.Application.Tasks.Dtos;

namespace TaskMaster.Application.Tasks.Queries.GetTaskById
{
    public record GetTaskByIdQuery(Guid Id) : IRequest<TaskDto?>;
}
