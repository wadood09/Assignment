namespace Assignment.Core.Domain.Entities
{
    public class Product : Auditables
    {
        public decimal Price { get; set; }
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public int AvailableStock { get; set; }
    }
}
