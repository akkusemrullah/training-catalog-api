using training_catalog_api.DTO.Category;
using training_catalog_api.Repositories.Category;

namespace training_catalog_api.Services.Category
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository categoryRepository;

        public CategoryService(ICategoryRepository _categoryRepository)
        {
            categoryRepository = _categoryRepository;
        }
        public async Task<int> CreateAsync(CategoryCreateDto dto)
        {
            Models.Category category = new Models.Category
            {
                CategoryName = dto.CategoryName
            };
            categoryRepository.AddCategoryAsync(category);
            Console.WriteLine(category.Id.ToString());
            return category.Id;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var category = await categoryRepository.GetCategoryByIdAsync(id);
            if (category != null)
            {
                await categoryRepository.DeleteCategoryAsync(id);
                return true;
            }
            return false;
        }

        public async Task<IEnumerable<Models.Category>> GetAllAsync()
        {
            var list = await categoryRepository.GetCategoriesListAsync();
            return list;
        }

        public async Task<Models.Category> GetByIdAsync(int id)
        {
            var category = await categoryRepository.GetCategoryByIdAsync(id);
            return category;
        }

        public async Task<bool> UpdateAsync(CategoryUpdateDto dto, int id)
        {
            var existCategory = await categoryRepository.GetCategoryByIdAsync(id);
            if (existCategory == null)
            {
                return false;
            }
            Models.Category updateCategory = new Models.Category
            {
                CategoryName = dto.CategoryName,
                Id = id
            };
            await categoryRepository.UpdateCategoryAsync(updateCategory);
            return true;
        }
    }
}