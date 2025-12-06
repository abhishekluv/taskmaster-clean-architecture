using MediatR;

using Microsoft.EntityFrameworkCore;

using TaskMaster.Application.Common.Interfaces;

namespace TaskMaster.Application.Tasks.Commands.RemoveTagFromTask
{
    public class RemoveTagFromTaskCommandHandler : IRequestHandler<RemoveTagFromTaskCommand, Unit>
    {
        private readonly IApplicationDbContext _context;

        public RemoveTagFromTaskCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(RemoveTagFromTaskCommand request, CancellationToken cancellationToken)
        {
            var task = await _context.TaskItems
            .Include(t => t.Tags)
            .FirstOrDefaultAsync(t => t.Id == request.TaskId, cancellationToken);

            if (task is null)
                throw new KeyNotFoundException($"Task {request.TaskId} not found.");

            var normalizedName = request.TagName.Trim();

            var tag = task.Tags.FirstOrDefault(t => t.Name == normalizedName);
            if (tag is null)
            {
                // Nothing to remove – you could also throw if you prefer
                return Unit.Value;
            }

            task.Tags.Remove(tag);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
