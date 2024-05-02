using Assignment.Models;
using Assignment.Models.OrderModel;

namespace Assignment.Core.Application.Interfaces.Services
{
    public interface IOrderService
    {
        Task<BaseResponse> CreateOrder(OrderRequest request);
        Task<BaseResponse<OrderResponse>> GetOrder(int id);
        Task<BaseResponse<ICollection<OrdersResponse>>> GetAllOrder();
        Task<BaseResponse> RemoveOrder(int id);
        Task<BaseResponse> UpdateOrder(int id, OrderRequest request);
    }
}
