using Assignment.Core.Domain.Entities;
using Assignment.Models.OrderModel;

namespace Assignment.Models.CustomerModel
{
    public class CustomerResponse
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
        public ICollection<CustomerOrderResponse> Orders { get; set; } = new List<CustomerOrderResponse>();
    }
}
