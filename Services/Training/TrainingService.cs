using Microsoft.EntityFrameworkCore.Metadata.Internal;
using training_catalog_api.DTO.Common;
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

        public async Task<SayfalamaDto<Models.Training>> GetAllAsync(TrainingListQuery q)
        {
            var page = q.Page < 1 ? 1 : q.Page;
            var pageSize = q.PageSize is < 1 or > 50 ? 10 : q.PageSize;
            var onlyPublished = q.IsPublished ?? true; // ödev gereği varsayılan true

            var (trainings, totalItems) = await trainingRepository.GetTrainingListAsync(
                new TrainingListQuery
                {
                    Search = q.Search,
                    CategoryId = q.CategoryId,
                    IsPublished = onlyPublished,
                    Page = page,
                    PageSize = pageSize
                });

            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            return new SayfalamaDto<Models.Training>
            {
                Items = trainings,
                Page = page,
                PageSize = pageSize,
                TotalItems = totalItems,
                TotalPages = totalPages
            };
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