namespace TaskMaster.WebApi.Dtos.Tasks
{
    public class UpdateTaskRequestDto
    {
        public string Title { get; set; } = default!;
        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }
        public string Priority { get; set; } = default!;

    }
}
