namespace Assignment.Core.Domain.Entities
{
    public class Customer : Auditables
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
