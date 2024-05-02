﻿namespace Assignment.Core.Domain.Entities
{
    public class User : Auditables
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; }
    }
}
