using MediatR;
using Microsoft.EntityFrameworkCore;
using OnionArch.Domain.Common.Repositories;

namespace OnionArch.Infrastructure.Common.EntityFramework.RequestHandlers
{
    public class SaveChangesRequestHandler<TDbContext> : IRequestHandler<SaveChangesRequest,int> where TDbContext : DbContext
    {
        private readonly TDbContext _dbContext;

        public SaveChangesRequestHandler(TDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> Handle(SaveChangesRequest request, CancellationToken cancellationToken)
        {
            return await _dbContext.SaveChangesAsync(request.CancellationToken);
        }
    }
}
