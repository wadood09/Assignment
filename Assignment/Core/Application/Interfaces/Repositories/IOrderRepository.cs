using Assignment.Core.Domain.Entities;
using System.Linq.Expressions;

namespace Assignment.Core.Application.Interfaces.Repositories
{
    public interface IOrderRepository
    {
        Task<Order> AddAsync(Order order);
        Task<Order> GetAsync(int id);
        Task<Order> GetAsync(Expression<Func<Order, bool>> exp);
        Task<ICollection<Order>> GetAllAsync();
        Task<ICollection<Order>> GetSelectedAsync(Expression<Func<Order, bool>> exp);
        void Remove(Order order);
        Order Update(Order order);
    }
}
