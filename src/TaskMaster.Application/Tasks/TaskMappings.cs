using TaskMaster.Application.Tasks.Dtos;
using TaskMaster.Domain.Entities;

namespace TaskMaster.Application.Tasks
{
    public static class TaskMappings
    {
        public static TaskDto ToDto(this TaskItem task)
        {
            return new TaskDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                DueDate = task.DueDate,
                Status = task.Status.ToString(),
                Priority = task.Priority.ToString(),
                AssignedToUserId = task.AssignedToUserId
            };
        }
    }
}
