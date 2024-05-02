using Assignment.Core.Application.Interfaces.Repositories;
using Assignment.Core.Domain.Entities;
using Assignment.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Assignment.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly SalesContext _context;

        public ProductRepository(SalesContext context)
        {
            _context = context;
        }

        public async Task<Product> AddAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            return product;
        }

        public async Task<bool> ExistsAsync(string name)
        {
            var product = await _context.Products.AnyAsync(prod => string.Equals(prod.Name, name, StringComparison.OrdinalIgnoreCase));
            return product;
        }

        public async Task<bool> ExistsAsync(string name, int id)
        {
            var product = await _context.Products.AnyAsync(prod => string.Equals(prod.Name, name, StringComparison.OrdinalIgnoreCase) && prod.Id != id);
            return product;
        }

        public async Task<ICollection<Product>> GetAllAsync()
        {
            var products = await _context.Products.ToListAsync();
            return products;
        }

        public async Task<Product> GetAsync(int id)
        {
            var product = await _context.Products.SingleOrDefaultAsync(pro => pro.Id == id);
            return product;
        }

        public async Task<Product> GetAsync(Expression<Func<Product, bool>> exp)
        {
            var product = await _context.Products.SingleOrDefaultAsync(exp);
            return product;
        }

        public async Task<ICollection<Product>> GetSelectedAsync(Expression<Func<Product, bool>> exp)
        {
            var products = await _context.Products.Where(exp).ToListAsync();
            return products;
        }

        public void Remove(Product product)
        {
            _context.Products.Remove(product);
        }

        public Product Update(Product product)
        {
            _context.Products.Update(product);
            return product;
        }
    }
}
