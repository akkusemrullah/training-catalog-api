
using Microsoft.EntityFrameworkCore;
using training_catalog_api.Data;

namespace training_catalog_api.Repositories.Training
{
    public class TrainingRepository : ITrainingRepository
    {
        private readonly AppDbContext context;

        public TrainingRepository(AppDbContext _context)
        {
            context = _context;
        }
        public async Task AddTrainingAsync(Models.Training training)
        {
            context.Trainings.Add(training);
            await context.SaveChangesAsync();
        }

        public async Task DeleteTrainingAsync(int id)
        {
            var training = await context.Trainings.FirstOrDefaultAsync(x => x.Id == id);
            if (training != null)
            {
                context.Trainings.Remove(training);
                await context.SaveChangesAsync();
            }
        }

        public async Task<Models.Training?> GetTrainingByIdAsync(int id)
        {

            var training = await context.Trainings.AsNoTracking().Include(x => x.Category).FirstOrDefaultAsync(x => x.Id == id);
            if (training == null)
            {
                Console.WriteLine("Trainin bulunamadı.");
                return null;
            }
            Console.WriteLine("Trainin bulundu.");
            return training;
        }

        public async Task<(List<Models.Training> Trainings, int TotalItems)> GetTrainingListAsync(TrainingListQuery q)
        {
            var query = context.Trainings
                .AsNoTracking()
                .Include(t => t.Category)  // kategori adı gösterilecekse
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(q.Search))
            {
                var pat = $"%{q.Search.Trim()}%";
                query = query.Where(t => EF.Functions.Like(t.Title, pat));
            }

            if (q.CategoryId.HasValue)
                query = query.Where(t => t.CategoryId == q.CategoryId.Value);

            if (q.IsPublished ?? true)     // default: sadece yayınlananlar
                query = query.Where(t => t.IsPublished);

            var totalItems = await query.CountAsync();

            var skip = Math.Max(q.Page - 1, 0) * q.PageSize;
            var trainings = await query
                .OrderByDescending(t => t.CreatedAt) // kararlı sıralama önerilir
                .Skip(skip)
                .Take(q.PageSize)
                .ToListAsync();

            return (trainings, totalItems);
        }

        public async Task UpdateTrainingAsync(Models.Training training)
        {
            context.Trainings.Update(training);
            await context.SaveChangesAsync();
        }
    }
}