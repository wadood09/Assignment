using Assignment.Core.Application.Interfaces.Repositories;
using Assignment.Core.Domain.Entities;
using Assignment.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Xml.Linq;

namespace Assignment.Infrastructure.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly SalesContext _context;
        public RoleRepository(SalesContext context)
        {
            _context = context;
        }

        public async Task<Role> AddAsync(Role role)
        {
            await _context.Roles.AddAsync(role);
            return role;
        }

        public async Task<bool> ExistAsync(string name)
        {
            return await _context.Roles.AnyAsync(a => a.Name == name);
        }

        public async Task<bool> ExistAsync(string name, int id)
        {
            return await _context.Roles.AnyAsync(a => a.Name == name && a.Id != id);
        }

        public async Task<ICollection<Role>> GetAllAsync()
        {
            return await _context.Roles
                .Include(a => a.Users)
                .ToListAsync();
        }

        public async Task<Role> GetAsync(Expression<Func<Role, bool>> exp)
        {
            var role = await _context.Roles
                 .Include(a => a.Users)
                 .FirstOrDefaultAsync(exp);
            return role;
        }

        public async Task<Role> GetAsync(int id)
        {
            var role = await _context.Roles
                 .Include(a => a.Users)
                 .FirstOrDefaultAsync(a => a.Id == id);
            return role;
        }

        public void Remove(Role role)
        {
            _context.Roles.Remove(role);
        }

        public Role Update(Role entity)
        {
            var role = _context.Roles.Update(entity);
            return entity;
        }
    }
}
