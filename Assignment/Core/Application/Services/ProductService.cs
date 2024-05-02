using Assignment.Core.Application.Interfaces.Repositories;
using Assignment.Core.Application.Interfaces.Services;
using Assignment.Core.Domain.Entities;
using Assignment.Models;
using Assignment.Models.ProductModel;
using System.Security.Claims;

namespace Assignment.Core.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IHttpContextAccessor _httpContext;
        private readonly IUserRepository _userRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IProductRepository productRepository, IUnitOfWork unitOfWork, IHttpContextAccessor httpContext, IUserRepository userRepository)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
            _httpContext = httpContext;
            _userRepository = userRepository;
        }

        public async Task<BaseResponse> CreateProduct(ProductRequest request)
        {
            var exists = await _productRepository.ExistsAsync(request.Name);
            if(exists)
            {
                return new BaseResponse
                {
                    Message = $"Product with name '{request.Name}' already exists",
                    IsSuccessful = false
                };
            }

            var loginUserId = _httpContext.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
            var product = new Product
            {
                AvailableStock = request.Quantity,
                Description = request.Description ?? "This product has no description",
                Name = request.Name,
                Price = request.Price,
                CreatedBy = loginUserId
            };

            await _productRepository.AddAsync(product);
            await _unitOfWork.SaveAsync();

            return new BaseResponse
            {
                Message = "Product created successfully",
                IsSuccessful = true
            };
        }

        public async Task<BaseResponse<ICollection<ProductResponse>>> GetAllProduct()
        {
            var products = await _productRepository.GetAllAsync();

            return new BaseResponse<ICollection<ProductResponse>>
            {
                Message = "List of products",
                IsSuccessful = true,
                Value = products.Select(prod => new ProductResponse
                {
                    Id = prod.Id,
                    Description = prod.Description ?? "This product has no description",
                    Name = prod.Name,
                    Price = prod.Price,
                    DateCreated = prod.DateCreated,
                    ManagerName = _userRepository.GetAsync(int.Parse(prod.CreatedBy)).Result.FirstName + " " + _userRepository.GetAsync(int.Parse(prod.CreatedBy)).Result.LastName
                }).ToList()
            };
        }

        public async Task<BaseResponse<ProductResponse>> GetProduct(int id)
        {
            var product = await _productRepository.GetAsync(id);
            var creator = await _userRepository.GetAsync(int.Parse(product.CreatedBy));
            if (product == null)
            {
                return new BaseResponse<ProductResponse>
                {
                    Message = "Product does not exists",
                    IsSuccessful = false
                };
            }

            return new BaseResponse<ProductResponse>
            {
                Message = "Product successfully found",
                IsSuccessful = true,
                Value = new ProductResponse
                {
                    Id = product.Id,
                    Name = product.Name,
                    ManagerName = creator.FirstName + " " + creator.LastName,
                    Price = product.Price,
                    DateCreated = product.DateCreated,
                    Description = product.Description,
                }
            };
        }

        public async Task<BaseResponse> RemoveProduct(int id)
        {
            var product = await _productRepository.GetAsync(id);
            if (product == null)
            {
                return new BaseResponse
                {
                    Message = "Product does not exists",
                    IsSuccessful = false
                };
            }

            _productRepository.Remove(product);
            await _unitOfWork.SaveAsync();

            return new BaseResponse
            {
                Message = "Product deleted successfully",
                IsSuccessful = true
            };
        }

        public async Task<BaseResponse> UpdateProduct(int id, ProductRequest request)
        {
            var exists = await _productRepository.ExistsAsync(request.Name, id);
            if (exists)
            {
                return new BaseResponse
                {
                    Message = $"Product with name '{request.Name}' already exists",
                    IsSuccessful = false
                };
            }

            var product = await _productRepository.GetAsync(id);
            if (product == null)
            {
                return new BaseResponse
                {
                    Message = $"Product with id '{id}' does not exists",
                    IsSuccessful = false
                };
            }
            var loginUserId = _httpContext.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value.ToString();

            product.Name = request.Name;
            product.Description = request.Description ?? "This product has no description";
            product.Price = request.Price;
            product.AvailableStock = request.Quantity;
            product.DateModified = DateTime.Now;
            product.ModifiedBy = loginUserId;
            
            _productRepository.Update(product);
            await _unitOfWork.SaveAsync();

            return new BaseResponse
            {
                Message = "Update Successfull",
                IsSuccessful = true
            };
        }
    }
}
