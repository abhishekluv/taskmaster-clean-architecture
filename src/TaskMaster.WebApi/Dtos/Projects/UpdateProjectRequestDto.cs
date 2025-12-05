namespace TaskMaster.WebApi.Dtos.Projects
{
    public class UpdateProjectRequestDto
    {
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
    }
}
