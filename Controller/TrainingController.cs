using System.Linq;
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
        public async Task<IActionResult> GetAllTrainings(
            [FromQuery] string? search,
            [FromQuery] int? categoryId,
            [FromQuery] bool? isPublished,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await trainingService.GetAllAsync(new TrainingListQuery
            {
                Search = search,
                CategoryId = categoryId,
                IsPublished = isPublished,   // null ise service default true uygular
                Page = pageNumber,
                PageSize = pageSize
            });

            // ÖNEMLİ: 200 + boş liste döndür (UI "No results" göstersin)
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTrainingById(int id)
        {
            var training = await trainingService.GetByIdAsync(id);
            if (training == null)
            {
                return NotFound();
            }
            return Ok(training);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTraining(DTO.Training.TrainingCreateDto training)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int id = await trainingService.CreateAsync(training);
            if (id <= 0)
            {
                return BadRequest();
            }

            var createdTraining = await trainingService.GetByIdAsync(id);
            return CreatedAtAction(nameof(GetTrainingById), new { id }, createdTraining);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTraining(DTO.Training.TrainingUpdateDto training, int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            bool result = await trainingService.UpdateAsync(training, id);
            return result ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTraining(int id)
        {
            bool result = await trainingService.DeleteAsync(id);
            return result ? NoContent() : NotFound();
        }
    }
}