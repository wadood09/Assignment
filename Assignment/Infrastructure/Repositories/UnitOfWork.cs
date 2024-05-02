using Assignment.Core.Application.Interfaces.Repositories;
using Assignment.Infrastructure.Context;

namespace Assignment.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SalesContext _context;

        public UnitOfWork(SalesContext context)
        {
            _context = context;
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
