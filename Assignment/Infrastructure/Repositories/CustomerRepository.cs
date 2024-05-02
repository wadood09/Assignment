using Assignment.Core.Application.Interfaces.Repositories;
using Assignment.Core.Domain.Entities;
using Assignment.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Mysqlx.Expr;
using System.Linq.Expressions;

namespace Assignment.Infrastructure.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly SalesContext _context;
        public CustomerRepository(SalesContext context)
        {
            _context = context;
        }
        public async Task<Customer> AddAsync(Customer customer)
        {
            await _context.Customers.AddAsync(customer);
            return customer;
        }

        public async Task<bool> ExistsAsync(string email, int id)
        {
            var exists = await _context.Customers.AnyAsync(cus => cus.User.Email == email && cus.Id != id);
            return exists;
        }

        public async Task<ICollection<Customer>> GetAllAsync()
        {
            return await _context.Customers.Include(cu => cu.User).ToListAsync();
        }

        public async Task<Customer> GetAsync(Expression<Func<Customer, bool>> exp)
        {
            return await _context.Customers.Include(cu => cu.User)
                .Include(cu => cu.Orders).ThenInclude(cu => cu.Items)
                .FirstOrDefaultAsync(exp);
        }

        public async Task<Customer> GetAsync(int id)
        {
            return await _context.Customers.Include(cu => cu.User)
                .Include(cu => cu.Orders).ThenInclude(cu => cu.Items)
                .FirstOrDefaultAsync(cu => cu.Id == id);
        }

        public void Remove(Customer customer)
        {
            _context.Customers.Remove(customer);
        }

        public Customer Update(Customer customer)
        {
            _context.Customers.Update(customer);
            return customer;
        }
    }
}
