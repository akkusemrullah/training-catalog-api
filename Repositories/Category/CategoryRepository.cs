
using Microsoft.EntityFrameworkCore;
using training_catalog_api.Data;

namespace training_catalog_api.Repositories.Category{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext context;

        public CategoryRepository(AppDbContext _context)
        {
            context = _context;
        }
        public async Task AddCategory(Models.Category category)
        {
            context.Categories.Add(category);
            await context.SaveChangesAsync();
        }

        public async Task DeleteCategory(Models.Category category)
        {
            context.Categories.Remove(category);
            await context.SaveChangesAsync();
        }

        public async Task<List<Models.Category>> GetCategories()
        {
            var categoryList = context.Categories.AsNoTracking().ToListAsync();
            return await categoryList;
        }

        public async Task<Models.Category> GetCategoryById(int id)
        {
            var category = context.Categories.Include(c=> c.Trainings).FirstOrDefaultAsync(x => x.Id == id);
            return await category;
        }

        public async Task UpdateCategory(Models.Category category)
        {
            context.Categories.Update(category);
            await context.SaveChangesAsync();
        }
    }
}