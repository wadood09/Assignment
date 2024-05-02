using Assignment.Models.RoleModel;
using Assignment.Models;
using Assignment.Models.UserModel;

namespace Assignment.Core.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<BaseResponse> CreateUser(UserRequest request);
        Task<BaseResponse<UserResponse>> GetUser(int id);
        Task<BaseResponse<ICollection<UserResponse>>> GetAllUsers();
        Task<BaseResponse> RemoveUser(int id);
        Task<BaseResponse> UpdateUser(int id, UserRequest request);
        Task<BaseResponse<UserResponse>> Login(LoginRequestModel model);
    }
}
