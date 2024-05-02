using Assignment.Core.Domain.Entities;
using Assignment.Models.ProductModel;

namespace Assignment.Models.OrderModel
{
    public class OrderResponse
    {
        public int Id { get; set; }
        public string SalespersonName { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime DateCreated { get; set; }
        public string? Description { get; set; }
        public ICollection<OrderItemResponse> Items { get; set; } = new List<OrderItemResponse>();
    }

    public class OrderItemResponse
    {
        public int Id { get; set; }
        public string ProductName { get; set; } = default!;
        public int ProductId { get; set; }
        public decimal UnitPrice { get; set; }
        public int Units { get; set; }
    }
}
