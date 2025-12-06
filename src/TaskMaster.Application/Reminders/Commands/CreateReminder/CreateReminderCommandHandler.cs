using MediatR;

using Microsoft.EntityFrameworkCore;

using TaskMaster.Application.Common.Interfaces;
using TaskMaster.Domain.Entities;

namespace TaskMaster.Application.Reminders.Commands.CreateReminder
{
    public class CreateReminderCommandHandler : IRequestHandler<CreateReminderCommand, Guid>
    {
        private readonly IApplicationDbContext _context;

        public CreateReminderCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> Handle(CreateReminderCommand request, CancellationToken cancellationToken)
        {
            var task = await _context.TaskItems
                .FirstOrDefaultAsync(t => t.Id == request.TaskId, cancellationToken);

            if (task is null)
                throw new KeyNotFoundException($"Task {request.TaskId} not found.");

            var reminder = new Reminder
            {
                TaskId = request.TaskId,
                ReminderTime = request.ReminderTime,
                IsSent = false
            };

            _context.Reminders.Add(reminder);
            await _context.SaveChangesAsync(cancellationToken);

            return reminder.Id;
        }
    }
}
