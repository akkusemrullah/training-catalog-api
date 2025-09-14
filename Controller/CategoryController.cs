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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var category = await categoryService.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return Ok(category);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory(DTO.Category.CategoryCreateDto category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int id = await categoryService.CreateAsync(category);
            if (id <= 0)
            {
                return BadRequest();
            }

            var createdCategory = await categoryService.GetByIdAsync(id);
            return CreatedAtAction(nameof(GetCategoryById), new { id }, createdCategory);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(DTO.Category.CategoryUpdateDto category, int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool result = await categoryService.UpdateAsync(category, id);
            return result ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            bool result = await categoryService.DeleteAsync(id);
            return result ? NoContent() : NotFound();
        }
    }
}