using Assignment.Core.Domain.Entities;

namespace Assignment.Models.OrderModel
{
    public class OrderRequest
    {
        public ICollection<ItemRequest> Items { get; set; } = new List<ItemRequest>();
        public int CustomerId { get; set; }
        public string? Description { get; set; }
    }

    public class ItemRequest
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
