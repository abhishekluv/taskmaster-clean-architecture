namespace TaskMaster.WebApi.Dtos.Tasks
{
    public class AssignTagToTaskRequestDto
    {
        public string TagName { get; set; } = default!;
        public string? Color { get; set; }
    }
}
