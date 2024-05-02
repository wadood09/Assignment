using Assignment.Core.Domain.Entities;
using System.Linq.Expressions;

namespace Assignment.Core.Application.Interfaces.Repositories
{
    public interface IProductRepository
    {
        Task<Product> AddAsync(Product product);
        Task<bool> ExistsAsync(string name);
        Task<bool> ExistsAsync(string name, int id);
        Task<Product> GetAsync(int id);
        Task<Product> GetAsync(Expression<Func<Product, bool>> exp);
        Task<ICollection<Product>> GetAllAsync();
        Task<ICollection<Product>> GetSelectedAsync(Expression<Func<Product, bool>> exp);
        void Remove(Product product);
        Product Update(Product product);
    }
}
