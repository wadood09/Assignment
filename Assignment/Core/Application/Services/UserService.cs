using Assignment.Core.Application.Interfaces.Repositories;
using Assignment.Core.Application.Interfaces.Services;
using Assignment.Core.Domain.Entities;
using Assignment.Models;
using Assignment.Models.UserModel;
using System.Security.Claims;

namespace Assignment.Core.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContext;
        private readonly ICustomerRepository _customerRepository;
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUserRepository userRepository, IRoleRepository roleRepository, IUnitOfWork unitOfWork, ICustomerRepository customerRepository, IHttpContextAccessor httpContext)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _unitOfWork = unitOfWork;
            _customerRepository = customerRepository;
            _httpContext = httpContext;
        }

        public async Task<BaseResponse> CreateUser(UserRequest request)
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

            var role = await _roleRepository.GetAsync(request.RoleId);
            if (role == null)
            {
                return new BaseResponse
                {
                    Message = "Role does not exists",
                    IsSuccessful = false
                };
            }

            var user = new User
            {
                Email = request.Email,
                Password = request.Password,
                FirstName = request.FirstName,
                LastName = request.LastName,
                DateOfBirth = request.DateOfBirth,
                RoleId = role.Id,
                Role = role,
                CreatedBy = "1"
            };

            role.Users.Add(user);
            _roleRepository.Update(role);
            var newUser = await _userRepository.AddAsync(user);

            if (role.Name == "Customer")
            {
                var customer = new Customer
                {
                    User = newUser,
                    UserId = newUser.Id,
                    CreatedBy = "1"
                };
                await _customerRepository.AddAsync(customer);
            }
            await _unitOfWork.SaveAsync();

            return new BaseResponse
            {
                Message = "User created successfully",
                IsSuccessful = true
            };
        }

        public async Task<BaseResponse<ICollection<UserResponse>>> GetAllUsers()
        {
            var users = await _userRepository.GetAllAsync();

            return new BaseResponse<ICollection<UserResponse>>
            {
                Message = "List of users",
                IsSuccessful = true,
                Value = users.Select(user => new UserResponse
                {
                    Id = user.Id,
                    FullName = user.FirstName + " " + user.LastName,
                    Age = DateTime.Now.Year - user.DateOfBirth.Year,
                    Email = user.Email,
                    RoleId = user.RoleId,
                    RoleName = user.Role.Name,
                }).ToList(),
            };
        }

        public async Task<BaseResponse<UserResponse>> GetUser(int id)
        {
            var user = await _userRepository.GetAsync(id);
            if (user == null)
            {
                return new BaseResponse<UserResponse>
                {
                    Message = "User not found",
                    IsSuccessful = false
                };
            }

            return new BaseResponse<UserResponse>
            {
                Message = "User successfully found",
                IsSuccessful = true,
                Value = new UserResponse
                {
                    Id = user.Id,
                    FullName = user.FirstName + " " + user.LastName,
                    Age = DateTime.Now.Year - user.DateOfBirth.Year,
                    Email = user.Email,
                    RoleId = user.RoleId,
                    RoleName = user.Role.Name
                }
            };
        }

        public async Task<BaseResponse> RemoveUser(int id)
        {
            var user = await _userRepository.GetAsync(id);
            if (user == null)
            {
                return new BaseResponse
                {
                    Message = "User does not exist",
                    IsSuccessful = false
                };
            }

            _userRepository.Remove(user);
            await _unitOfWork.SaveAsync();

            return new BaseResponse
            {
                Message = "User deleted successfully",
                IsSuccessful = true
            };
        }

        public async Task<BaseResponse> UpdateUser(int id, UserRequest request)
        {
            var user = await _userRepository.GetAsync(id);
            if (user == null)
            {
                return new BaseResponse
                {
                    Message = "User does not exist",
                    IsSuccessful = false
                };
            }

            var formerRole = await _roleRepository.GetAsync(user.RoleId);
            formerRole.Users.Remove(user);
            _roleRepository.Update(formerRole);

            var exists = await _userRepository.ExistsAsync(request.Email, id);
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

            var role = await _roleRepository.GetAsync(request.RoleId);
            if (role == null)
            {
                return new BaseResponse
                {
                    Message = $"Role with id '{request.RoleId}' does not exists",
                    IsSuccessful = false
                };
            }

            var loginUserId = _httpContext.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.Email = request.Email;
            user.Password = request.Password;
            user.DateOfBirth = request.DateOfBirth;
            user.RoleId = request.RoleId;
            user.Role = role;
            user.DateModified = DateTime.Now;
            user.ModifiedBy = loginUserId;

            role.Users.Add(user);

            _roleRepository.Update(role);
            _userRepository.Update(user);
            await _unitOfWork.SaveAsync();

            return new BaseResponse
            {
                Message = "User updated successfully",
                IsSuccessful = true
            };
        }

        public async Task<BaseResponse<UserResponse>> Login(LoginRequestModel model)
        {
            var user = await _userRepository.GetAsync(user => user.Email == model.Email && user.Password == model.Password);
            if (user == null)
            {
                return new BaseResponse<UserResponse>
                {
                    Message = "Invalid Credentials",
                    IsSuccessful = false
                };
            }

            return new BaseResponse<UserResponse>
            {
                Message = "Login Successfull",
                IsSuccessful = true,
                Value = new UserResponse
                {
                    Id = user.Id,
                    FullName = user.FirstName + " " + user.LastName,
                    Age = DateTime.Now.Year - user.DateOfBirth.Year,
                    Email = user.Email,
                    RoleId = user.RoleId,
                    RoleName = user.Role.Name
                }
            };
        }
    }
}
