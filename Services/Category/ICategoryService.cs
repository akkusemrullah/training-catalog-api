namespace training_catalog_api.Services.Category
{
    public interface ICategoryService
    {
        Task<IEnumerable<Models.Category>> GetAllAsync();
        Task<Models.Category> GetByIdAsync(int id);
        Task<int> CreateAsync(DTO.Category.CategoryCreateDto dto);
        Task<bool> UpdateAsync(DTO.Category.CategoryUpdateDto dto, int id);
        Task<bool> DeleteAsync(int id);
    }
}