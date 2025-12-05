namespace TaskMaster.WebApi.Dtos.Projects
{
    public class CreateProjectRequestDto
    {
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
    }
}
