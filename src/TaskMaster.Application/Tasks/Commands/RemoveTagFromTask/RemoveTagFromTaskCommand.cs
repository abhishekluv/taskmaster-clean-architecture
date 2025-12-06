using MediatR;

namespace TaskMaster.Application.Tasks.Commands.RemoveTagFromTask
{
    public record RemoveTagFromTaskCommand(
        Guid TaskId,
        string TagName) : IRequest<Unit>;
}
