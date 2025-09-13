using Microsoft.EntityFrameworkCore.Metadata.Internal;
using training_catalog_api.DTO.Training;
using training_catalog_api.Repositories.Training;

namespace training_catalog_api.Services.Training
{
    public class TrainingService : ITrainingService
    {
        private readonly ITrainingRepository trainingRepository;

        public TrainingService(ITrainingRepository _trainingRepository)
        {
            trainingRepository = _trainingRepository;
        }
        public async Task<int> CreateAsync(TrainingCreateDto dto)
        {
            Models.Training training = new Models.Training
            {
                Title = dto.Title,
                ShortDescription = dto.ShortDescription,
                LongDescription = dto.LongDescription,
                CategoryId = dto.CategoryId,
                ImageUrl = dto.ImageUrl,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                IsPublished = dto.IsPublished,
                CreatedAt = dto.CreatedAt,
                UpdatedAt = dto.UpdatedAt
            };

            trainingRepository.AddTrainingAsync(training);
            Console.WriteLine(training.Id.ToString());
            return training.Id;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var training = trainingRepository.GetTrainingAsync(id);

            if (training == null)
            {
                return false;
            }

            trainingRepository.DeleteTrainingAsync(id);
            return true;
        }

        public async Task<IEnumerable<Models.Training>> GetAllAsync()
        {
            var list = await trainingRepository.GetTrainingListAsync();
            return list;
        }

        public async Task<Models.Training> GetByIdAsync(int id)
        {
            var category = await trainingRepository.GetTrainingAsync(id);
            return category;
        }

        public async Task<bool> UpdateAsync(TrainingUpdateDto dto, int id)
        {
            var existTraining = await trainingRepository.GetTrainingAsync(id);
            if (existTraining == null)
            {
                return false;
            }
            Models.Training updateTraining = new Models.Training
            {
                Title = dto.Title,
                ShortDescription = dto.ShortDescription,
                LongDescription = dto.LongDescription,
                CategoryId = dto.CategoryId,
                ImageUrl = dto.ImageUrl,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                IsPublished = dto.IsPublished,
                CreatedAt = dto.CreatedAt,
                UpdatedAt = dto.UpdatedAt,
                Id = id
            };
            await trainingRepository.UpdateTrainingAsync(updateTraining);
            return true;
        }
    }
}