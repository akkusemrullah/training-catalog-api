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

            await trainingRepository.AddTrainingAsync(training);
            Console.WriteLine(training.Id.ToString());
            return training.Id;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var training = await trainingRepository.GetTrainingByIdAsync(id);

            if (training == null)
            {
                return false;
            }

            await trainingRepository.DeleteTrainingAsync(id);
            return true;
        }

        public async Task<IEnumerable<Models.Training>> GetAllAsync(int pageNumber, int pageSize)
        {
            var list = await trainingRepository.GetTrainingListAsync(pageNumber, pageSize);
            return list;
        }

        public async Task<Models.Training> GetByIdAsync(int id)
        {
            var training = await trainingRepository.GetTrainingByIdAsync(id);
            return training;
        }

        public async Task<bool> UpdateAsync(TrainingUpdateDto dto, int id)
        {
            var existTraining = await trainingRepository.GetTrainingByIdAsync(id);
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