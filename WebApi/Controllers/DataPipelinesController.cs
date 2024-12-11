using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Security.Claims;
using Dapper;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DataPipelinesController : ControllerBase
    {
        private readonly IDataPipelineService _pipelineService;
        private readonly IMapper _mapper;

        public DataPipelinesController(IDataPipelineService pipelineService, IMapper mapper)
        {
            _pipelineService = pipelineService;
            _mapper = mapper;
        }

        // GET: api/datapipelines
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DataPipelineDto>>> GetAll()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var pipelines = await _pipelineService.GetAllPipelinesAsync(userId);
            var pipelineDtos = _mapper.Map<IEnumerable<DataPipelineDto>>(pipelines);
            return Ok(pipelineDtos);
        }

        // GET: api/datapipelines/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<DataPipelineDto>> GetById(Guid id)
        {
            var pipeline = await _pipelineService.GetPipelineByIdAsync(id);
            if (pipeline == null)
                return NotFound();

            var pipelineDto = _mapper.Map<DataPipelineDto>(pipeline);
            return Ok(pipelineDto);
        }

        // POST: api/datapipelines
        [HttpPost]
        public async Task<ActionResult<DataPipelineDto>> Create([FromBody] DataPipelineDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var pipeline = await _pipelineService.CreatePipelineAsync(dto.Name, userId);
            var pipelineDto = _mapper.Map<DataPipelineDto>(pipeline);
            return CreatedAtAction(nameof(GetById), new { id = pipelineDto.Id }, pipelineDto);
        }

        // PUT: api/datapipelines/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDataFlow(Guid id, [FromBody] UpdateDataFlowDto dto)
        {
            try
            {
                var updatedPipeline = await _pipelineService.UpdatePipelineDataFlowAsync(id, dto.DataFlowJson);
                var updatedPipelineDto = _mapper.Map<DataPipelineDto>(updatedPipeline);
                return Ok(updatedPipelineDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/datapipelines/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _pipelineService.DeletePipelineAsync(id);
            return NoContent();
        }
        [HttpPost("dataflow")]
        public async Task<IActionResult> AddDataFlow([FromBody] AddDataFlowDto dto)
        {
            if (dto.Id == Guid.Empty)
            {
                return BadRequest("Invalid GUID provided.");
            }

            if (string.IsNullOrWhiteSpace(dto.DataFlowJson))
            {
                return BadRequest("DataFlowJson cannot be empty.");
            }

            try
            {
                // Przykład: Aktualizacja istniejącego DataPipeline z danymi flow
                var updatedPipeline = await _pipelineService.UpdatePipelineDataFlowAsync(dto.Id, dto.DataFlowJson);

                if (updatedPipeline == null)
                {
                    return NotFound($"DataPipeline with ID {dto.Id} not found.");
                }

                var updatedPipelineDto = _mapper.Map<DataPipelineDto>(updatedPipeline);
                return Ok(updatedPipelineDto);
            }
            catch (Exception ex)
            {
                // Logowanie błędu (opcjonalnie)
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpPost("saveDataToWarehouse")]
        public async Task<IActionResult> SaveDataToWarehouse([FromBody] DataWarehouseDto dto)
        {
            if (dto == null || dto.Id == Guid.Empty)
            {
                return BadRequest("Nieprawidłowe dane żądania lub brak Id pipeline'u.");
            }

            if (dto.Columns == null || dto.Columns.Count == 0)
            {
                return BadRequest("Brak definicji kolumn w przesłanych danych.");
            }

            if (dto.Rows == null || dto.Rows.Count == 0)
            {
                return BadRequest("Brak wierszy do zapisania w hurtowni.");
            }

            // Nazwa tabeli zależna od Id pipeline'u
            var tableName = $"Pipeline_{dto.Id:N}"; // N - format GUID bez kresek

            try
            {
                using (var connection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=DataWarehouse-mmm;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False"))
                {
                    await connection.OpenAsync();

                    var checkTableCmdText = $@"
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[{tableName}]') AND type in (N'U'))
BEGIN
    CREATE TABLE [{tableName}] (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        {string.Join(", ", dto.Columns.ConvertAll(c => $"[{c}] NVARCHAR(255) NULL"))}
    );
END
";

                    using (var checkTableCmd = new SqlCommand(checkTableCmdText, connection))
                    {
                        await checkTableCmd.ExecuteNonQueryAsync();
                    }

                    // Wstawianie danych
                    foreach (var row in dto.Rows)
                    {
                        var columnsList = string.Join(", ", dto.Columns.ConvertAll(c => $"[{c}]"));
                        var valuesList = string.Join(", ", dto.Columns.ConvertAll(c => $"@{c}"));

                        var insertCmdText = $@"
INSERT INTO [{tableName}] ({columnsList})
VALUES ({valuesList})
";

                        using (var insertCmd = new SqlCommand(insertCmdText, connection))
                        {
                            foreach (var col in dto.Columns)
                            {
                                object val = DBNull.Value;
                                if (row.ContainsKey(col))
                                {
                                    var cellValue = row[col];
                                    if (cellValue is System.Text.Json.JsonElement jsonEl)
                                    {
                                        // Konwersja JsonElement na prosty typ
                                        if (jsonEl.ValueKind == System.Text.Json.JsonValueKind.Number)
                                        {
                                            val = jsonEl.GetDouble();
                                        }
                                        else if (jsonEl.ValueKind == System.Text.Json.JsonValueKind.String)
                                        {
                                            val = jsonEl.GetString();
                                        }
                                        else
                                        {
                                            // Dla innych typów
                                            val = jsonEl.ToString();
                                        }
                                    }
                                    else
                                    {
                                        // Jeśli nie jest JsonElement, używamy wartości bezpośrednio
                                        val = cellValue ?? DBNull.Value;
                                    }
                                }

                                insertCmd.Parameters.AddWithValue($"@{col}", val ?? DBNull.Value);
                            }

                            // WYKONANIE INSERT
                            await insertCmd.ExecuteNonQueryAsync();
                        }
                    }
                }

                return Ok(new { message = "Dane zapisane w hurtowni danych." });
            }
            catch (Exception ex)
            {
                // Logowanie błędu
                return StatusCode(500, new { error = "Błąd podczas zapisu do hurtowni danych.", details = ex.Message });
            }
        }
        [HttpGet("getDataFromWarehouse")]
        public async Task<IActionResult> GetDataFromWarehouse([FromQuery] string datapipelineid)
        {
            // Usuwamy myślniki z GUID i tworzymy nazwę tabeli
            var tableName = $"Pipeline_{datapipelineid:N}";

            // Tworzymy dynamiczne zapytanie do tabeli
            var sql = $"SELECT * FROM {tableName}";

            using (var connection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=DataWarehouse-mmm;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False"))
            {
                try
                {
                    var data = await connection.QueryAsync(sql);
                    return Ok(data);
                }
                catch (SqlException ex)
                {
                    // Obsługa błędów np. gdy tabela nie istnieje
                    return BadRequest($"Błąd zapytania SQL: {ex.Message}");
                }
            }
        }
    }
    public class DataWarehouseDto
    {
        public Guid Id { get; set; }
        public List<Dictionary<string, object>> Rows { get; set; }
        public List<string> Columns { get; set; }
    }
    public class AddDataFlowDto
    {
        public Guid Id { get; set; }
        public string DataFlowJson { get; set; }
    }
}
