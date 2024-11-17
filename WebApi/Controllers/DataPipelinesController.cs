
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataPipelinesController : ControllerBase
    {
    //    private readonly IDataPipelineService _dataPipelineService;
    //    public DataPipelinesController(IDataPipelineService dataPipelineService)
    //    {
    //        _dataPipelineService = dataPipelineService;
    //    }

    //    [SwaggerOperation(Summary = "Retrieves all data pipelines")]
    //    [HttpGet]
    //    public IActionResult Get()
    //    {
    //        var dataPipelines = _dataPipelineService.GetAllDataPipeLines();
    //        return Ok(dataPipelines);
    //    }
    //    [SwaggerOperation(Summary = "Retrieves a specific data pipeline by unique id")]
    //    [HttpGet("{id}")]
    //    public IActionResult Get(Guid id)
    //    {
    //        var dataPipeline = _dataPipelineService.GetDataPipeLine(id);
    //        if(dataPipeline == null)
    //        {
    //            return NotFound();
    //        }
    //        return Ok(dataPipeline);

    //    }
    //    [SwaggerOperation(Summary = "create a new data pipeline")]
    //    [HttpPost]
    //    public IActionResult Create(CreateDataPipelineDto newDataPipeline)
    //    {
    //        var dataPipeline = _dataPipelineService.AddNewDataPipeline(newDataPipeline);
    //        return Created($"api/posts/{dataPipeline.Id}", dataPipeline);
    //    }

    //    [SwaggerOperation(Summary = "Update a existing data pipeline")]
    //    [HttpPut]
    //    public IActionResult Update(UpdateDataPipelineDto updateDataPipeline)
    //    {
    //        _dataPipelineService.UpdateDataPipeline(updateDataPipeline);
    //        return NoContent();
    //    }

    //    [SwaggerOperation(Summary ="Delete a specific data pipeline")]
    //    [HttpDelete]
    //    public IActionResult Delete(Guid id)
    //    {
    //        _dataPipelineService.DeleteDataPipeline(id);
    //        return NoContent();
    //    }
    }
}
