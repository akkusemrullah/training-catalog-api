using Microsoft.AspNetCore.Mvc;
using training_catalog_api.Services.Category;

namespace training_catalog_api.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService categoryService;

        public CategoryController(ICategoryService _categoryService)
        {
            categoryService = _categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await categoryService.GetAllAsync();
            if (!categories.Any())
            {
                return NoContent();
            }
            return Ok(categories);
        }

        [HttpGet("id")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var category = await categoryService.GetByIdAsync(id);
            if (category != null)
            {
                return Ok(category);
            }
            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory(DTO.Category.CategoryCreateDto category)
        {
            int result = await categoryService.CreateAsync(category);
            return result >= 0 ? Ok() : BadRequest();
        }

        [HttpPut("id")]
        public async Task<IActionResult> UpdateCategory(DTO.Category.CategoryUpdateDto category, int id)
        {
            bool result = await categoryService.UpdateAsync(category, id);
            return result == true ? Ok() : BadRequest();
        }

        [HttpDelete("id")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            bool result = await categoryService.DeleteAsync(id);
            Console.WriteLine(result);
            return result == true ? Ok() : BadRequest();
        }
    }
}