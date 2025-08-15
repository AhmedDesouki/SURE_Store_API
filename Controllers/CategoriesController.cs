using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SURE_Store_API.DTOs.Category;
using SURE_Store_API.DTOs;
using SURE_Store_API.Services;


namespace SURE_Store_API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {

        private readonly ICategoryService _categoryService;


        /// <param name="categoryService">Category service for category operations</param>
        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;  // Store category service reference
        }


        [HttpGet]
        public async Task<ActionResult<List<CategoryDto>>> GetCategories()
        {
            var categories = await _categoryService.GetCategoriesAsync();
            return Ok(categories);
        }


        /// <param name="id">Category ID</param>

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDto>> GetCategory(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }


        /// <param name="request">Category creation request</param>

        [HttpPost]  // HTTP POST endpoint
        [Authorize(Roles = "Admin")]  // Require admin role
        public async Task<ActionResult<CategoryDto>> CreateCategory(CreateCategoryDto request)
        {
            var category = await _categoryService.CreateCategoryAsync(request);
            return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, category);
        }

        /// <param name="id">Category ID</param>
        /// <param name="request">Category update request</param>

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<CategoryDto>> UpdateCategory(int id, UpdateCategoryRequest request)
        {
            try
            {
                var category = await _categoryService.UpdateCategoryAsync(id, request);
                return Ok(category);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }


        /// <param name="id">Category ID</param>

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteCategory(int id)
        {
            try
            {
                var success = await _categoryService.DeleteCategoryAsync(id);
                if (!success)
                {
                    return NotFound();
                }

                return NoContent();  // Return 204 for successful deletion
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}
