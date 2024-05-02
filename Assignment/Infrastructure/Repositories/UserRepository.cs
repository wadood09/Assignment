using Assignment.Core.Application.Interfaces.Repositories;
using Assignment.Core.Domain.Entities;
using Assignment.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Assignment.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly SalesContext _context;
        public UserRepository(SalesContext context)
        {
            _context = context;
        }

        public async Task<User> AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            return await _context.Users.OrderByDescending(user => user.DateCreated).FirstOrDefaultAsync();
        }

        public async Task<bool> ExistsAsync(string email)
        {
            return await _context.Users.AnyAsync(x => x.Email == email);
        }

        public async Task<bool> ExistsAsync(string email, int id)
        {
            return await _context.Users.AnyAsync(x => x.Email == email && x.Id != id);
        }

        public async Task<ICollection<User>> GetAllAsync()
        {
            return await _context.Users.Include(user => user.Role).ToListAsync();
        }

        public async Task<User> GetAsync(int id)
        {
            var user = await _context.Users.Include(a => a.Role).SingleOrDefaultAsync(a => a.Id == id);
            return user;
        }

        public async Task<User> GetAsync(Expression<Func<User, bool>> exp)
        {
            var user = await _context.Users.Include(a => a.Role).FirstOrDefaultAsync(exp);
            return user;
        }

        public void Remove(User user)
        {
            _context.Users.Remove(user);
        }

        public User Update(User entity)
        {
            _context.Users.Update(entity);
            return entity;
        }
    }
}
