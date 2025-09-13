namespace training_catalog_api.Services.Training
{
    public interface ITrainingService
    {
        Task<IEnumerable<Models.Training>> GetAllAsync();
        Task<Models.Training> GetByIdAsync(int id);
        Task<int> CreateAsync(DTO.Training.TrainingCreateDto dto);
        Task<bool> UpdateAsync(DTO.Training.TrainingUpdateDto dto, int id);
        Task<bool> DeleteAsync(int id);
    }
}