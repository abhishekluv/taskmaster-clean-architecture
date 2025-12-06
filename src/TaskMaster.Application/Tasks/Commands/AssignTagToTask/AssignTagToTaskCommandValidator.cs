using FluentValidation;

namespace TaskMaster.Application.Tasks.Commands.AssignTagToTask
{
    public class AssignTagToTaskCommandValidator : AbstractValidator<AssignTagToTaskCommand>
    {
        public AssignTagToTaskCommandValidator()
        {
            RuleFor(x => x.TaskId).NotEmpty();

            RuleFor(x => x.TagName)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Color)
                .MaximumLength(7)
                .When(x => !string.IsNullOrWhiteSpace(x.Color));
        }
    }
}
