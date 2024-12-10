using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DataPipelinesController : ControllerBase
    {
        private readonly IDataPipelineService _dataPipelineService;

        public DataPipelinesController(IDataPipelineService dataPipelineService)
        {
            _dataPipelineService = dataPipelineService;
        }

        // GET: api/DataPipelines/{id}
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<DataPipelineDto>> Get(Guid id)
        {
            var pipeline = await _dataPipelineService.GetDataPipelineByIdAsync(id);
            if (pipeline == null)
                return NotFound();

            return Ok(pipeline);
        }

        // GET: api/DataPipelines/user/{userId}
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<DataPipelineDto>>> GetAllByUserId(string userId)
        {
            var pipelines = await _dataPipelineService.GetAllDataPipelinesByUserIdAsync(userId);
            return Ok(pipelines);
        }

        // POST: api/DataPipelines
        [HttpPost]
        public async Task<ActionResult<DataPipelineDto>> Create([FromBody] DataPipelineDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = await _dataPipelineService.CreateDataPipelineAsync(dto);
            // Zwracamy 201 + Location nagłówek na świeżo utworzony zasób
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        // PUT: api/DataPipelines/{id}
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<DataPipelineDto>> Update(Guid id, [FromBody] DataPipelineDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Upewniamy się, że ID w dto jest zgodne z tym z URL
            if (id != dto.Id)
                return BadRequest("IDs do not match.");

            var updated = await _dataPipelineService.UpdateDataPipelineAsync(dto);
            if (updated == null)
                return NotFound();

            return Ok(updated);
        }

        // DELETE: api/DataPipelines/{id}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _dataPipelineService.DeleteDataPipelineAsync(id);
            // Jeśli usuwanie zakończyło się powodzeniem, zwracamy 204 (No Content)
            return NoContent();
        }
    }
}
