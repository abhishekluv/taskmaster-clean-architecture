using MediatR;

using Microsoft.EntityFrameworkCore;

using TaskMaster.Application.Common.Interfaces;
using TaskMaster.Domain.Entities;
using TaskMaster.Domain.Enums;

using TaskStatus = TaskMaster.Domain.Enums.TaskStatus;


namespace TaskMaster.Application.Tasks.Commands.CreateTask
{
    public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, Guid>
    {
        private readonly IApplicationDbContext _context;

        public CreateTaskCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
        {
            var project = await _context.Projects
                            .FirstOrDefaultAsync(p => p.Id == request.ProjectId, cancellationToken);

            if (project is null)
                throw new KeyNotFoundException($"Project {request.ProjectId} not found");

            var task = new TaskItem
            {
                ProjectId = request.ProjectId,
                Title = request.Title,
                Description = request.Description,
                DueDate = request.DueDate,
                Priority = Enum.Parse<TaskPriority>(request.Priority),
                Status = TaskStatus.Todo,
                AssignedToUserId = request.UserId
            };

            _context.TaskItems.Add(task);
            await _context.SaveChangesAsync(cancellationToken);

            return task.Id;
        }
    }
}
