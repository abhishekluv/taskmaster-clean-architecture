using TaskMaster.Domain.Common;

namespace TaskMaster.Domain.Entities
{
    public class Project : AuditableEntity
    {
        public string Name { get; set; } = default!;
        public string? Description { get; set; }

        public string OwnerUserId { get; set; } = default!;

        //navigation property
        public List<TaskItem> Tasks { get; set; } = new();
    }
}
