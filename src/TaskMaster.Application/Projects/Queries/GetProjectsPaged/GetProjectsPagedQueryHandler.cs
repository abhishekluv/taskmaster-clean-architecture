using MediatR;

using Microsoft.EntityFrameworkCore;

using TaskMaster.Application.Common.Interfaces;
using TaskMaster.Application.Common.Models;
using TaskMaster.Application.Projects.Dtos;

namespace TaskMaster.Application.Projects.Queries.GetProjectsPaged
{
    public class GetProjectsPagedQueryHandler : IRequestHandler<GetProjectsPagedQuery, PagedResult<ProjectDto>>
    {
        private readonly IApplicationDbContext _context;

        public GetProjectsPagedQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<PagedResult<ProjectDto>> Handle(GetProjectsPagedQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Projects
                                .Include(x => x.Tasks)
                                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.OwnerUserId))
            {
                query = query.Where(x => x.OwnerUserId == request.OwnerUserId);
            }

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                var search = request.Search.Trim();

                query = query.Where(x => x.Name.Contains(search) || 
                            (x.Description != null && x.Description.Contains(search)));
            }

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                         .OrderBy(x => x.Name)
                         .Skip((request.PageNumber - 1) * request.PageSize)
                         .Take(request.PageSize)
                         .ToListAsync(cancellationToken);

            var dtoItems = items.Select(x => x.ToDto()).ToList();

            return new PagedResult<ProjectDto>(dtoItems, request.PageNumber, request.PageSize, totalCount);
        }
    }
}
