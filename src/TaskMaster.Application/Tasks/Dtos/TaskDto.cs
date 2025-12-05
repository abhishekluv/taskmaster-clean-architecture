namespace TaskMaster.Application.Tasks.Dtos
{
    public class TaskDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = default!;
        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }
        public string Status { get; set; } = default!;
        public string Priority { get; set; } = default!;
        public string? AssignedToUserId { get; set; }
    }
}
