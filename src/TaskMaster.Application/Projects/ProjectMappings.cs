using TaskMaster.Application.Projects.Dtos;
using TaskMaster.Domain.Entities;

namespace TaskMaster.Application.Projects
{
    public static class ProjectMappings
    {
        public static ProjectDto ToDto(this Project project)
        {
            return new ProjectDto
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                TaskCount = project.Tasks?.Count ?? 0
            };
        }
    }
}
