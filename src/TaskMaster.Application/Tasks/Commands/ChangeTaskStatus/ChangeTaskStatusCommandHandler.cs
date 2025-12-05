using MediatR;

using Microsoft.EntityFrameworkCore;

using TaskMaster.Application.Common.Interfaces;

using TaskStatus = TaskMaster.Domain.Enums.TaskStatus;

namespace TaskMaster.Application.Tasks.Commands.ChangeTaskStatus
{
    public class ChangeTaskStatusCommandHandler : IRequestHandler<ChangeTaskStatusCommand, Unit>
    {
        private readonly IApplicationDbContext _context;

        public ChangeTaskStatusCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(ChangeTaskStatusCommand request, CancellationToken cancellationToken)
        {
            var task = await _context.TaskItems.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (task is null) throw new KeyNotFoundException($"Task {request.Id} not found");

            task.Status = Enum.Parse<TaskStatus>(request.Status);
            task.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
