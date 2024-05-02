using Assignment.Core.Application.Interfaces.Services;
using Assignment.Core.Domain.Entities;
using Assignment.Models.OrderModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Assignment.Controllers
{
    [Route("api/orders")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        [Authorize(Roles = "Manager, Salesperson")]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _orderService.GetAllOrder();
            if (orders.Value.Count == 0)
            {
                return NotFound("No orders have been made");
            }
            return Ok(orders.Value);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Manager, Salesperson")]
        public async Task<IActionResult> GetOrder([FromRoute] int id)
        {
            var order = await _orderService.GetOrder(id);
            if (order.IsSuccessful)
            {
                return Ok(order.Value);
            }
            return NotFound(order.Message);
        }

        [HttpPost]
        [Authorize(Roles = "Salesperson")]
        public async Task<IActionResult> CreateOrder([FromBody] OrderRequest request)
        {
            var order = await _orderService.CreateOrder(request);
            if (order.IsSuccessful)
            {
                return Ok(order.Message);
            }
            return BadRequest(order.Message);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Salesperson")]
        public async Task<IActionResult> UpdateOrder([FromRoute] int id, [FromBody] OrderRequest request)
        {
            var order = await _orderService.UpdateOrder(id, request);
            if (order.IsSuccessful)
            {
                return Ok(order.Message);
            }
            return BadRequest(order.Message);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Salesperson")]
        public async Task<IActionResult> DeleteOrder([FromRoute] int id)
        {
            var order = await _orderService.RemoveOrder(id);
            if (order.IsSuccessful)
            {
                return Ok(order.Message);
            }
            return BadRequest(order.Message);
        }
    }
}
