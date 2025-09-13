using training_catalog_api.Models;
namespace training_catalog_api.Repositories.Training
{
    public interface ITrainingRepository
    {
        Task<List<Models.Training>> GetTrainingListAsync();
        Task<Models.Training> GetTrainingAsync(int id);
        Task AddTrainingAsync(Models.Training training);
        Task UpdateTrainingAsync(Models.Training training);
        Task DeleteTrainingAsync(int id);
    }
}