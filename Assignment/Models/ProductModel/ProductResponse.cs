using Assignment.Core.Domain.Entities;

namespace Assignment.Models.ProductModel
{
    public class ProductResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string ManagerName { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
