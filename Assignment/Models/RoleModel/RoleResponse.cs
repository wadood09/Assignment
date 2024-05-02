using Assignment.Core.Domain.Entities;

namespace Assignment.Models.RoleModel
{
    public class RoleResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
    }
}
