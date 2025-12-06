using MediatR;

using Microsoft.EntityFrameworkCore;

using TaskMaster.Application.Common.Interfaces;
using TaskMaster.Domain.Entities;

namespace TaskMaster.Application.Tasks.Commands.AssignTagToTask
{
    public class AssignTagToTaskCommandHandler : IRequestHandler<AssignTagToTaskCommand, Unit>
    {
        private readonly IApplicationDbContext _context;
        public AssignTagToTaskCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(AssignTagToTaskCommand request, CancellationToken cancellationToken)
        {
            var task = await _context.TaskItems
            .Include(t => t.Tags)
            .FirstOrDefaultAsync(t => t.Id == request.TaskId, cancellationToken);

            if (task is null)
                throw new KeyNotFoundException($"Task {request.TaskId} not found.");

            var normalizedName = request.TagName.Trim();

            var tag = await _context.Tags
                .FirstOrDefaultAsync(t => t.Name == normalizedName, cancellationToken);

            if (tag is null)
            {
                tag = new Tag
                {
                    Name = normalizedName,
                    Color = string.IsNullOrWhiteSpace(request.Color)
                        ? "#000000"
                        : request.Color
                };

                _context.Tags.Add(tag);
            }

            // If already assigned, do nothing (idempotent)
            if (!task.Tags.Any(t => t.Id == tag.Id))
            {
                task.Tags.Add(tag);
                await _context.SaveChangesAsync(cancellationToken);
            }

            return Unit.Value;
        }
    }
}
