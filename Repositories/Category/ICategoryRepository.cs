using training_catalog_api.Models;

namespace training_catalog_api.Repositories.Category{

    public interface ICategoryRepository{
        Task<List<Models.Category>> GetCategories();
        Task<Models.Category> GetCategoryById(int id);
        Task AddCategory(Models.Category category);
        Task UpdateCategory(Models.Category category);
        Task DeleteCategory(Models.Category category);   
    }
}