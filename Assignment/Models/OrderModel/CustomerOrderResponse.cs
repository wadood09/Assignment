using Assignment.Models.ProductModel;

namespace Assignment.Models.OrderModel
{
    public class CustomerOrderResponse
    {
        public int Id { get; set; }
        public ICollection<OrderItemResponse> Items { get; set; } = new List<OrderItemResponse>();
        public string SalespersonName { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime DateCreated { get; set; }
        public string? Description { get; set; }
    }
}
