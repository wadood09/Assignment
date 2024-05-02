using Assignment.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Assignment.Infrastructure.Context
{
    public class SalesContext : DbContext
    {
        public SalesContext(DbContextOptions<SalesContext> opt) : base(opt)
        {
            
        }

        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<User> Users => Set<User>();


        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            base.ConfigureConventions(configurationBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Customer>().Property<int>("Id").ValueGeneratedOnAdd();
            modelBuilder.Entity<Order>().Property<int>("Id").ValueGeneratedOnAdd();
            modelBuilder.Entity<Product>().Property<int>("Id").ValueGeneratedOnAdd();
            modelBuilder.Entity<Role>().Property<int>("Id").ValueGeneratedOnAdd();
            modelBuilder.Entity<User>().Property<int>("Id").ValueGeneratedOnAdd();
            

            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, DateCreated = DateTime.Now, Name = "Admin", CreatedBy = "1" },
                new Role { Id = 2, DateCreated = DateTime.Now, Name = "Manager", CreatedBy = "1" },
                new Role { Id = 3, DateCreated = DateTime.Now, Name = "Salesperson", CreatedBy = "1" },
                new Role { Id = 4, DateCreated = DateTime.Now, Name = "Customer", CreatedBy = "1" }
                );

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    DateCreated = DateTime.Now,
                    DateOfBirth = new DateTime(2008, 03, 19),
                    FirstName = "Olaniyi",
                    LastName = "Wadood",
                    Email = "admin@gmail.com",
                    Password = "admin",
                    RoleId = 1,
                    CreatedBy = "1"
                });
        }
    }
}
