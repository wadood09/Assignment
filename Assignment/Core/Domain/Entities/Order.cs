using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Assignment.Core.Domain.Entities
{
    public class Order : Auditables
    {
        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public string? Description { get; set; }
    }

    public class OrderItem
    {
        public int Id { get; set; }
        [Required]
        public string ProductName { get; set; } = default!;
        public int ProductId { get; set; }
        public decimal UnitPrice { get; set; }
        public int Units { get; set; }
        private static int count;
        public OrderItem()
        {
            Id = ++count;
        }
    }
}
