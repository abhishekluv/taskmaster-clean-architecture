using FluentValidation;

namespace TaskMaster.Application.Reminders.Commands.CreateReminder
{
    public class CreateReminderCommandValidator : AbstractValidator<CreateReminderCommand>
    {
        public CreateReminderCommandValidator()
        {
            RuleFor(x => x.TaskId).NotEmpty();

            RuleFor(x => x.ReminderTime)
                .NotEmpty();
            // If you want, later we can enforce ReminderTime >= now
            // RuleFor(x => x.ReminderTime)
            //     .Must(t => t > DateTime.UtcNow)
            //     .WithMessage("Reminder time must be in the future.");
        }
    }
}
