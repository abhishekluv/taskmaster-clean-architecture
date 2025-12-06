using MediatR;

namespace TaskMaster.Application.Reminders.Commands.CreateReminder
{
    public record CreateReminderCommand(
        Guid TaskId,
        DateTime ReminderTime) : IRequest<Guid>;
}
