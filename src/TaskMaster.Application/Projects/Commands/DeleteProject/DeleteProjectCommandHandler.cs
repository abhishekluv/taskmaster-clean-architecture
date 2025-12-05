using MediatR;

using Microsoft.EntityFrameworkCore;

using TaskMaster.Application.Common.Interfaces;

namespace TaskMaster.Application.Projects.Commands.DeleteProject
{
    public class DeleteProjectCommandHandler : IRequestHandler<DeleteProjectCommand, Unit>
    {
        private readonly IApplicationDbContext _context;

        public DeleteProjectCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
        {
            var project = await _context.Projects.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (project == null) throw new KeyNotFoundException($"Project {request.Id} not found");

            _context.Projects.Remove(project);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
