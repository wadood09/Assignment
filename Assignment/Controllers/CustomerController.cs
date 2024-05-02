using Assignment.Core.Application.Interfaces.Services;
using Assignment.Models.CustomerModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Assignment.Controllers
{
    [Route("api/customers")]
    [ApiController]
    [Authorize]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetAllCustomer()
        {
            var customers = await _customerService.GetAllCustomer();
            if (customers.Value.Count == 0)
            {
                return NotFound();
            }
            return Ok(customers.Value);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetCustomer([FromRoute] int id)
        {
            var customer = await _customerService.GetCustomer(id);
            if (customer.IsSuccessful)
            {
                return Ok(customer.Value);
            }
            return NotFound(customer.Message);
        }

        [HttpPost]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> CreateCustomer([FromForm] CustomerRequest request)
        {
            var customer = await _customerService.CreateCustomer(request);
            if (customer.IsSuccessful)
            {
                return Ok(customer.Message);
            }
            return BadRequest(customer.Message);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> UpdateCustomer([FromRoute] int id, [FromForm] CustomerRequest request)
        {
            var customer = await _customerService.UpdateCustomer(id, request);
            if (customer.IsSuccessful)
            {
                return Ok(customer.Message);
            }
            return BadRequest(customer.Message);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> DeleteCustomer([FromRoute] int id)
        {
            var customer = await _customerService.RemoveCustomer(id);
            if (customer.IsSuccessful)
            {
                return Ok(customer.Message);
            }
            return BadRequest(customer.Message);
        }

        [HttpGet("{id}/orders")]
        [Authorize(Roles = "Manager, Salesperson")]
        public async Task<IActionResult> GetCustomerOrder([FromRoute] int id)
        {
            var customer = await _customerService.GetCustomer(id);
            if (customer.IsSuccessful)
            {
                if (customer.Value.Orders.Any())
                    return Ok(customer.Value.Orders);
                return NotFound("This customer has made no orders");
            }
            return NotFound(customer.Message);
        }
    }
}
