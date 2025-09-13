using training_catalog_api.Models;

namespace training_catalog_api.Repositories.Category{

    public interface ICategoryRepository{
        Task<List<Models.Category>> GetCategoriesAsync();
        Task<Models.Category> GetCategoryByIdAsync(int id);
        Task AddCategoryAsync(Models.Category category);
        Task UpdateCategoryAsync(Models.Category category);
        Task DeleteCategoryAsync(Models.Category category);   
    }
}