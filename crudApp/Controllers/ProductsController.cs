using Microsoft.AspNetCore.Mvc;
using crudApp.Services.ProductService;
using crudApp.Services.ProductService.DTOs;

namespace crudApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {

        private readonly IProductService _productService; // inject the product service

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        // Get products list
        [HttpGet]
        public IActionResult Get()
        {
            var list = _productService.GetAllProducts();
            return Ok(list);
        }

        // Create product
        [HttpPost]
        public IActionResult Post(ProductRequestDTO request)
        {
            try
            {
                var result = _productService.CreateProduct(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Update product
        [HttpPut("{id}")]
        public IActionResult Put(int id, ProductRequestDTO request)
        {
            try
            {
                var result = _productService.UpdateProduct(id, request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Delete product
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var result = _productService.DeleteProduct(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
