using FluentValidation;

namespace TaskMaster.Application.Tasks.Commands.RemoveTagFromTask
{
    public class RemoveTagFromTaskCommandValidator : AbstractValidator<RemoveTagFromTaskCommand>
    {
        public RemoveTagFromTaskCommandValidator()
        {
            RuleFor(x => x.TaskId).NotEmpty();
            RuleFor(x => x.TagName).NotEmpty().MaximumLength(100);
        }
    }
}
