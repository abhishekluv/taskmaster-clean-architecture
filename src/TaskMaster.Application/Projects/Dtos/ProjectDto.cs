namespace TaskMaster.Application.Projects.Dtos
{
    public class ProjectDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public int TaskCount { get; set; }
    }
}
