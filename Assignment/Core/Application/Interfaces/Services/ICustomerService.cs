using Assignment.Models.CustomerModel;
using Assignment.Models;

namespace Assignment.Core.Application.Interfaces.Services
{
    public interface ICustomerService
    {
        Task<BaseResponse> CreateCustomer(CustomerRequest request);
        Task<BaseResponse<CustomerResponse>> GetCustomer(int id);
        Task<BaseResponse<ICollection<CustomersResponse>>> GetAllCustomer();
        Task<BaseResponse> RemoveCustomer(int id);
        Task<BaseResponse> UpdateCustomer(int id, CustomerRequest request);
    }
}
