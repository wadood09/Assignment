using Assignment.Core.Domain.Entities;
using System.Linq.Expressions;

namespace Assignment.Core.Application.Interfaces.Repositories
{
    public interface ICustomerRepository
    {
        Task<Customer> AddAsync(Customer customer);
        Task<bool> ExistsAsync(string email, int id);
        Task<Customer> GetAsync(int id);
        Task<Customer> GetAsync(Expression<Func<Customer, bool>> exp);
        Task<ICollection<Customer>> GetAllAsync();
        void Remove(Customer customer);
        Customer Update(Customer customer);
    }
}
