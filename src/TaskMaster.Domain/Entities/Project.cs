using TaskMaster.Domain.Common;

namespace TaskMaster.Domain.Entities
{
    public class Project : AuditableEntity
    {
        public string Name { get; set; } = default!;
        public string? Descriptions { get; set; }

        public string OwnerUserId { get; set; } = default!;

        //navigation property
        
    }
}
