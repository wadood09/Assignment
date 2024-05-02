using Assignment.Core.Application.Interfaces.Repositories;
using Assignment.Core.Application.Interfaces.Services;
using Assignment.Core.Domain.Entities;
using Assignment.Models;
using Assignment.Models.OrderModel;
using Assignment.Models.ProductModel;
using System.Security.Claims;

namespace Assignment.Core.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IHttpContextAccessor _httpContext;
        private readonly ICustomerRepository _customerRepository;
        private readonly IUserRepository _userRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public OrderService(ICustomerRepository customerRepository, IOrderRepository orderRepository, IProductRepository productRepository, IUnitOfWork unitOfWork, IHttpContextAccessor httpContext, IUserRepository userRepository)
        {
            _customerRepository = customerRepository;
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
            _httpContext = httpContext;
            _userRepository = userRepository;
        }

        public async Task<BaseResponse> CreateOrder(OrderRequest request)
        {
            var customer = await _customerRepository.GetAsync(request.CustomerId);
            if (customer == null)
            {
                return new BaseResponse
                {
                    Message = $"Customer with id '{request.CustomerId}' does not exists",
                    IsSuccessful = false
                };
            }

            var items = new List<OrderItem>();
            foreach (var item in request.Items)
            {
                var product = await _productRepository.GetAsync(item.ProductId);
                if (product == null)
                {
                    return new BaseResponse
                    {
                        Message = $"Product with id '{item.ProductId}' does not exists",
                        IsSuccessful = false
                    };
                }

                if (item.Quantity > product.AvailableStock)
                {
                    return new BaseResponse
                    {
                        Message = $"Not enough products with id '{product.Id}' available in store",
                        IsSuccessful = false
                    };
                }

                product.AvailableStock -= item.Quantity;
                items.Add(new OrderItem
                {
                    ProductId = item.ProductId,
                    ProductName = product.Name,
                    UnitPrice = product.Price,
                    Units = item.Quantity
                });
                _productRepository.Update(product);
            }

            var loginUserId = _httpContext.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
            var order = new Order
            {
                CustomerId = request.CustomerId,
                Customer = customer,
                CreatedBy = loginUserId,
                Description = request.Description,
                Items = items
            };

            customer.Orders.Add(order);
            _customerRepository.Update(customer);
            await _orderRepository.AddAsync(order);
            await _unitOfWork.SaveAsync();

            return new BaseResponse
            {
                Message = "Order created successfully",
                IsSuccessful = true
            };
        }

        public async Task<BaseResponse<ICollection<OrdersResponse>>> GetAllOrder()
        {
            var orders = await _orderRepository.GetAllAsync();

            return new BaseResponse<ICollection<OrdersResponse>>
            {
                Message = "List of Orders",
                IsSuccessful = true,
                Value = orders.Select(ord => new OrdersResponse
                {
                    Id = ord.Id,
                    CustomerId = ord.CustomerId,
                    CustomerName = ord.Customer.User.FirstName + " " + ord.Customer.User.LastName,
                    DateCreated = ord.DateCreated,
                    TotalPrice = ord.Items.Select(x => x.UnitPrice).Sum(),
                    Description = ord.Description,
                    SalespersonName = _userRepository.GetAsync(int.Parse(ord.CreatedBy)).Result.FirstName + " " + _userRepository.GetAsync(int.Parse(ord.CreatedBy)).Result.LastName,
                }).ToList(),
            };
        }

        public async Task<BaseResponse<OrderResponse>> GetOrder(int id)
        {
            var order = await _orderRepository.GetAsync(id);
            var creator = await _userRepository.GetAsync(int.Parse(order.CreatedBy));
            if (order == null)
            {
                return new BaseResponse<OrderResponse>
                {
                    Message = "Order not found",
                    IsSuccessful = false
                };
            }
            return new BaseResponse<OrderResponse>
            {
                Message = "Order successfully found",
                IsSuccessful = true,
                Value = new OrderResponse
                {
                    CustomerId = order.CustomerId,
                    CustomerName = order.Customer.User.FirstName + " " + order.Customer.User.LastName,
                    DateCreated = order.DateCreated,
                    Id = order.Id,
                    TotalPrice = order.Items.Select(x => x.UnitPrice).Sum(),
                    Description = order.Description,
                    Items = order.Items.Select(x => new OrderItemResponse
                    {
                        Id = x.Id,
                        ProductId = x.ProductId,
                        ProductName = x.ProductName,
                        UnitPrice = x.UnitPrice,
                        Units = x.Units
                    }).ToList(),
                    SalespersonName = creator.FirstName + " " + creator.LastName,
                }
            };
        }

        public async Task<BaseResponse> RemoveOrder(int id)
        {
            var order = await _orderRepository.GetAsync(id);
            if (order == null)
            {
                return new BaseResponse
                {
                    Message = "Order does not exists",
                    IsSuccessful = false
                };
            }

            _orderRepository.Remove(order);
            await _unitOfWork.SaveAsync();

            return new BaseResponse
            {
                Message = "Order deleted successfully",
                IsSuccessful = true
            };
        }

        public async Task<BaseResponse> UpdateOrder(int id, OrderRequest request)
        {
            var order = await _orderRepository.GetAsync(id);
            if (order == null)
            {
                return new BaseResponse
                {
                    Message = "Order does not exists",
                    IsSuccessful = false
                };
            }

            var customer = await _customerRepository.GetAsync(request.CustomerId);
            if (customer == null)
            {
                return new BaseResponse
                {
                    Message = $"Update failed. Customer with id '{request.CustomerId}' does not exists",
                    IsSuccessful = false
                };
            }

            var p = order.Items;
            foreach (var item in p)
            {
                var product = await _productRepository.GetAsync(item.ProductId);
                product.AvailableStock += item.Units;
                _productRepository.Update(product);
            }

            var items = new List<OrderItem>();
            foreach (var item in request.Items)
            {
                var product = await _productRepository.GetAsync(item.ProductId);
                if (product == null)
                {
                    return new BaseResponse
                    {
                        Message = $"Product with id '{item.ProductId}' does not exists",
                        IsSuccessful = false
                    };
                }

                if (item.Quantity > product.AvailableStock)
                {
                    return new BaseResponse
                    {
                        Message = $"Not enough products with id '{product.Id}' available in store",
                        IsSuccessful = false
                    };
                }

                product.AvailableStock -= item.Quantity;
                items.Add(new OrderItem
                {
                    ProductId = item.ProductId,
                    ProductName = product.Name,
                    UnitPrice = product.Price,
                    Units = item.Quantity
                });
                _productRepository.Update(product);
            }

            var loginUserId = _httpContext.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value.ToString();

            order.Items = items;
            order.Customer = customer;
            order.ModifiedBy = loginUserId;
            order.CustomerId = customer.Id;
            order.DateModified = DateTime.Now;
            order.Description = request.Description;

            _orderRepository.Update(order);
            await _unitOfWork.SaveAsync();
            return new BaseResponse
            {
                Message = "Order updated successfully",
                IsSuccessful = true
            };
        }
    }
}
