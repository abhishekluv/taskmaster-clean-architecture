using TaskMaster.Domain.Common;
using TaskMaster.Domain.Enums;

using TaskStatus = TaskMaster.Domain.Enums.TaskStatus;

namespace TaskMaster.Domain.Entities
{
    public class TaskItem : AuditableEntity
    {
        public string Title { get; set; } = default!;
        public string? Description { get; set; }

        public DateTime? DueDate { get; set; }
        public TaskStatus Status { get; set; } = TaskStatus.Todo;
        public TaskPriority Priority { get; set; } = TaskPriority.Medium;

        public string AssignedToUserId { get; set; } = default!;

        // FK
        public Guid ProjectId { get; set; }
        public Project Project { get; set; } = default!;

        // Many-to-many Tags
        public ICollection<Tag> Tags { get; set; } = new List<Tag>();

        // One-to-many Reminders
        public ICollection<Reminder> Reminders { get; set; } = new List<Reminder>();
    }
}
