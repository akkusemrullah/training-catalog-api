using Microsoft.AspNetCore.Mvc;
using training_catalog_api.Services.Training;

namespace training_catalog_api.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class TrainingController : ControllerBase
    {
        private readonly ITrainingService trainingService;

        public TrainingController(ITrainingService _trainingService)
        {
            trainingService = _trainingService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTrainings()
        {
            var trainings = await trainingService.GetAllAsync();
            if (!trainings.Any())
            {
                return NoContent();
            }
            return Ok(trainings);
        }

        [HttpGet("id")]
        public async Task<IActionResult> GetTrainingById(int id)
        {
            var training = await trainingService.GetByIdAsync(id);
            if (training != null)
            {
                return Ok(training);
            }
            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> CreateTraining(DTO.Training.TrainingCreateDto training)
        {
            int result = await trainingService.CreateAsync(training);
            return result > 0 ? Ok() : BadRequest();
        }

        [HttpPut("id")]
        public async Task<IActionResult> UpdateTraining(DTO.Training.TrainingUpdateDto training, int id)
        {
            bool result = await trainingService.UpdateAsync(training, id);
            return result == true ? Ok() : BadRequest();
        }

        [HttpDelete("id")]
        public async Task<IActionResult> DeleteTraining(int id)
        {
            bool result = await trainingService.DeleteAsync(id);
            Console.WriteLine(result);
            return result == true ? Ok() : BadRequest();
        }
    }
}