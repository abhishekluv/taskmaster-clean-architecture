using TaskMaster.Domain.Common;

namespace TaskMaster.Domain.Entities
{
    public class Tag : Entity
    {
        public string Name { get; set; } = default!;
        public string Color { get; set; } = "#000000";

        // Many-to-many with TaskItem
        public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
    }
}
