using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SURE_Store_API.DTOs.Product;
using SURE_Store_API.DTOs;
using SURE_Store_API.Services;    // For ProductService

//i was have error here cuz i i use "using SURE_Store_API.Interfaces" instead of "using SURE_Store_API.Services"
// so i injected IProductService instead of ProductService

namespace SURE_Store_API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {

        private readonly IProductService _productService;  // Product service for handling product operations

        // Constructor for dependency injection of product service
        // This constructor is called by the DI container when creating the controller
        public ProductsController(IProductService productService)  // Inject product service
        {
            _productService = productService;  // Store product service reference
        }


        [HttpGet]
        public async Task<ActionResult<ProductListResponse>> GetProducts(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] int? categoryId = null,
            [FromQuery] string? search = null)
        {
            // Call product service to get paginated and filtered product list
            var response = await _productService.GetProductsAsync(page, pageSize, categoryId, search);  // Process product list request


            return Ok(response);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProduct(int id)
        {
            // Call product service to get product by ID
            var product = await _productService.GetProductByIdAsync(id);  // Process product detail request


            if (product == null)
                return NotFound();


            return Ok(product);
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ProductDto>> CreateProduct(CreateProductDto request)  // Accept product creation request
        {
            // Call product service to create new product
            var product = await _productService.CreateProductAsync(request);  // Process product creation request

            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }


        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ProductDto>> UpdateProduct(int id, UpdateProductRequest request)  // Accept product ID and update request
        {
            try
            {
                // Call product service to update existing product
                var product = await _productService.UpdateProductAsync(id, request);  // Process product update request


                return Ok(product);
            }
            catch (ArgumentException ex)
            {

                return NotFound(new { message = ex.Message });
            }
        }


        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            // Call product service to delete product
            var success = await _productService.DeleteProductAsync(id);  // Process product deletion request


            if (!success)
                return NotFound();


            return NoContent();
        }


        [HttpGet("categories")]
        public async Task<ActionResult<List<CategoryDto>>> GetCategories()  // Return list of all categories
        {
            // Call product service to get all categories
            var categories = await _productService.GetCategoriesAsync();  // Process category list request


            return Ok(categories);
        }
    }


}
