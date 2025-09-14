
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

        public async Task<List<Models.Training>> GetTrainingListAsync(int pageNumber, int pageSize)
        {
            return await context.Trainings
              .AsNoTracking()
              .Skip((pageNumber - 1) * pageSize)
              .Take(pageSize)
              .ToListAsync();
        }

        public async Task UpdateTrainingAsync(Models.Training training)
        {
            context.Trainings.Update(training);
            await context.SaveChangesAsync();
        }
    }
}