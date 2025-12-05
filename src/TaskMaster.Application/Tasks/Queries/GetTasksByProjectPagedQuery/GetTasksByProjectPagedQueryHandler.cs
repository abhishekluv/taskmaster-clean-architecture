using MediatR;

using Microsoft.EntityFrameworkCore;

using TaskMaster.Application.Common.Interfaces;
using TaskMaster.Application.Common.Models;
using TaskMaster.Application.Tasks.Dtos;

namespace TaskMaster.Application.Tasks.Queries.GetTasksByProjectPagedQuery
{
    public class GetTasksByProjectPagedQueryHandler : IRequestHandler<GetTasksByProjectPagedQuery, PagedResult<TaskDto>>
    {
        private readonly IApplicationDbContext _context;

        public GetTasksByProjectPagedQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<TaskDto>> Handle(GetTasksByProjectPagedQuery request, CancellationToken cancellationToken)
        {
            var query = _context.TaskItems.
                            Where(t => t.ProjectId == request.ProjectId);

            var total = await query.CountAsync(cancellationToken);

            var items = await query
                        .OrderBy(t => t.Title)
                        .Skip((request.PageNumber - 1) * request.PageSize)
                        .Take(request.PageSize)
                        .ToListAsync(cancellationToken);

            var dtoItems = items.Select(t => t.ToDto()).ToList();

            return new PagedResult<TaskDto>(dtoItems, request.PageNumber, request.PageSize, total);
                    
        }
    }
}
