using Assignment.Models.ProductModel;

namespace Assignment.Models.OrderModel
{
    public class OrdersResponse
    {
        public int Id { get; set; }
        public string SalespersonName { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime DateCreated { get; set; }
        public string? Description { get; set; }
    }
}
