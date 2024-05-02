using Assignment.Core.Application.Interfaces.Services;
using Assignment.Models.ProductModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Assignment.Controllers
{
    [Route("api/products")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productService.GetAllProduct();
            if (products.Value.Count == 0)
            {
                return NotFound("No products available at the moment");
            }
            return Ok(products.Value);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct([FromRoute] int id)
        {
            var product = await _productService.GetProduct(id);
            if (product.IsSuccessful)
            {
                return Ok(product.Value);
            }
            return NotFound(product.Message);
        }

        [HttpPost]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> CreateProduct([FromForm] ProductRequest request)
        {
            var product = await _productService.CreateProduct(request);
            if (product.IsSuccessful)
            {
                return Ok(product.Message);
            }
            return BadRequest(product.Message);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> UpdateProduct([FromRoute] int id, [FromForm] ProductRequest request)
        {
            var product = await _productService.UpdateProduct(id, request);
            if (product.IsSuccessful)
            {
                return Ok(product.Message);
            }
            return BadRequest(product.Message);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> DeleteProduct([FromRoute] int id)
        {
            var product = await _productService.RemoveProduct(id);
            if (product.IsSuccessful)
            {
                return Ok(product.Message);
            }
            return BadRequest(product.Message);
        }
    }
}
