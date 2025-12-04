using TaskMaster.Domain.Common;

namespace TaskMaster.Domain.Entities
{
    public class Reminder : Entity
    {
        public Guid TaskId { get; set; }
        public TaskItem Task { get; set; } = default!;

        public DateTime ReminderTime { get; set; }
        public bool IsSent { get; set; }

    }
}
