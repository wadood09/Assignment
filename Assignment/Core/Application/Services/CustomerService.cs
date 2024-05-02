using Assignment.Core.Application.Interfaces.Repositories;
using Assignment.Core.Application.Interfaces.Services;
using Assignment.Core.Domain.Entities;
using Assignment.Models;
using Assignment.Models.OrderModel;
using Assignment.Models.CustomerModel;
using System.Security.Claims;

namespace Assignment.Core.Application.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContext;

        public CustomerService(ICustomerRepository customerRepository, IUserRepository userRepository, IRoleRepository roleRepository, IUnitOfWork unitOfWork, IHttpContextAccessor httpContext)
        {
            _customerRepository = customerRepository;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _unitOfWork = unitOfWork;
            _httpContext = httpContext;
        }

        public async Task<BaseResponse> CreateCustomer(CustomerRequest request)
        {
            var exists = await _userRepository.ExistsAsync(request.Email);
            if (exists)
            {
                return new BaseResponse
                {
                    Message = "Email already exists!!!",
                    IsSuccessful = false
                };
            }

            if (request.Password != request.ConfirmPassword)
            {
                return new BaseResponse
                {
                    Message = "Password does not match",
                    IsSuccessful = false
                };
            }

            var role = await _roleRepository.GetAsync(r => r.Name == "Customer");
            if (role == null)
            {
                return new BaseResponse
                {
                    Message = "Customer Role does not exists",
                    IsSuccessful = false
                };
            }

            var loginUser = _httpContext.HttpContext.User.Claims.FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier)?.Value;
            var user = new User
            {
                Email = request.Email,
                Password = request.Password,
                FirstName = request.FirstName,
                LastName = request.LastName,
                DateOfBirth = request.DateOfBirth,
                RoleId = role.Id,
                Role = role,
                CreatedBy = loginUser
            };

            role.Users.Add(user);
            _roleRepository.Update(role);
            await _userRepository.AddAsync(user);

            var customer = new Customer()
            {
                UserId = user.Id,
                User = user,
                CreatedBy = loginUser
            };

            await _customerRepository.AddAsync(customer);
            await _unitOfWork.SaveAsync();

            return new BaseResponse
            {
                Message = "Registration Successfull!!!",
                IsSuccessful = true,
            };
        }

        public async Task<BaseResponse<ICollection<CustomersResponse>>> GetAllCustomer()
        {
            var customers = await _customerRepository.GetAllAsync();
            return new BaseResponse<ICollection<CustomersResponse>>
            {
                Message = "List of customers",
                IsSuccessful = true,
                Value = customers.Select(cu => new CustomersResponse
                {
                    Id = cu.Id,
                    FullName = cu.User.FirstName + " " + cu.User.LastName,
                    Email = cu.User.Email,
                    Age = DateTime.Now.Year - cu.User.DateOfBirth.Year
                }).ToList()
            };
        }

        public async Task<BaseResponse<CustomerResponse>> GetCustomer(int id)
        {
            var customer = await _customerRepository.GetAsync(id);
            if (customer == null)
            {
                return new BaseResponse<CustomerResponse>
                {
                    Message = "Customer not found",
                    IsSuccessful = false
                };
            }

            return new BaseResponse<CustomerResponse>
            {
                Message = "Customer found Successfully",
                IsSuccessful = true,
                Value = new CustomerResponse
                {
                    Id = customer.Id,
                    FullName = customer.User.FirstName + " " + customer.User.LastName,
                    Email = customer.User.Email,
                    Age = DateTime.Now.Year - customer.User.DateOfBirth.Year,
                    Orders = customer.Orders.Select(ord => new CustomerOrderResponse
                    {
                        Id = ord.Id,
                        SalespersonName = ord.CreatedBy,
                        Description = ord.Description,
                        Items = ord.Items.Select(item => new OrderItemResponse
                        {
                            Id = item.Id,
                            ProductId = item.ProductId,
                            ProductName = item.ProductName,
                            UnitPrice = item.UnitPrice,
                            Units = item.Units
                        }).ToList(),
                        TotalPrice = ord.Items.Select(item => item.UnitPrice).Sum(),
                        DateCreated = ord.DateCreated,
                    }).ToList()
                }
            };
        }

        public async Task<BaseResponse> RemoveCustomer(int id)
        {
            var customer = await _customerRepository.GetAsync(id);
            if (customer == null)
            {
                return new BaseResponse
                {
                    Message = "Customer does not exists",
                    IsSuccessful = false
                };
            }

            _customerRepository.Remove(customer);
            await _unitOfWork.SaveAsync();

            return new BaseResponse
            {
                Message = "Customer deleted successfully",
                IsSuccessful = true
            };
        }

        public async Task<BaseResponse> UpdateCustomer(int id, CustomerRequest request)
        {
            var customer = await _customerRepository.GetAsync(id);
            if (customer == null)
            {
                return new BaseResponse
                {
                    Message = "Customer does not exist",
                    IsSuccessful = false
                };
            }

            var exists = await _customerRepository.ExistsAsync(request.Email, id);
            if (exists)
            {
                return new BaseResponse
                {
                    Message = "Email already exists!!!",
                    IsSuccessful = false
                };
            }

            if (request.Password != request.ConfirmPassword)
            {
                return new BaseResponse
                {
                    Message = "Password does not match",
                    IsSuccessful = false
                };
            }
            var loginUser = _httpContext.HttpContext.User.Claims.FirstOrDefault(p => p.Type == ClaimTypes.Name)?.Value;

            customer.User.FirstName = request.FirstName;
            customer.User.LastName = request.LastName;
            customer.User.Email = request.Email;
            customer.User.Password = request.Password;
            customer.User.DateOfBirth = request.DateOfBirth;
            customer.User.DateModified = DateTime.Now;
            customer.User.ModifiedBy = loginUser;
            customer.DateModified = DateTime.Now;
            customer.ModifiedBy = loginUser;

            _customerRepository.Update(customer);

            await _unitOfWork.SaveAsync();

            return new BaseResponse
            {
                Message = "Customer updated successfully",
                IsSuccessful = true
            };
        }
    }
}
