using Assignment.Core.Domain.Entities;

namespace Assignment.Models.ProductModel
{
    public class ProductRequest
    {
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
