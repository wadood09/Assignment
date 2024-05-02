using Assignment.Core.Domain.Entities;

namespace Assignment.Models.RoleModel
{
    public class RoleRequest
    {
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
    }
}
