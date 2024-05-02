using Assignment.Models;
using Assignment.Models.ProductModel;

namespace Assignment.Core.Application.Interfaces.Services
{
    public interface IProductService
    {
        Task<BaseResponse> CreateProduct(ProductRequest request);
        Task<BaseResponse<ProductResponse>> GetProduct(int id);
        Task<BaseResponse<ICollection<ProductResponse>>> GetAllProduct();
        Task<BaseResponse> RemoveProduct(int id);
        Task<BaseResponse> UpdateProduct(int id, ProductRequest request);
    }
}
