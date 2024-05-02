using Assignment.Core.Domain.Entities;

namespace Assignment.Models.UserModel
{
    public class UserResponse
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
    }
}
