using Dapper;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ModelsController:ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _connectionString = "your_connection_string_here";
        public ModelsController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        [HttpGet("analyze/{pipelineId}")]
        public async Task<IActionResult> Analyze(string pipelineId)
        {
            // Sprawdzenie czy pipelineId to GUID
            if (!Guid.TryParse(pipelineId, out var guid))
            {
                return BadRequest("Invalid pipelineId GUID.");
            }

            // 1. Pobierz dane z bazy dla pipelineId
            var tableName = $"Pipeline_{guid:N}";
            var sql = $"SELECT * FROM {tableName}";

            List<IDictionary<string, object>> data;
            using (var connection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=DataWarehouse-mmm;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False"))
            {
                try
                {
                    // Zwracamy wyniki w postaci listy słowników
                    // Każdy wiersz: IDictionary<string,object> gdzie key = nazwa kolumny, value = wartość komórki
                    data = (await connection.QueryAsync(sql))
                        .Select(row => (IDictionary<string, object>)row)
                        .ToList();
                }
                catch (SqlException ex)
                {
                    // Obsługa błędów np. gdy tabela nie istnieje
                    return BadRequest($"Błąd zapytania SQL: {ex.Message}");
                }
            }

            if (data == null || data.Count == 0)
            {
                return NotFound($"No data found for pipeline {pipelineId}");
            }

            // 2. Przygotuj payload do mikroserwisu Pythonowego
            // columns -> listę nazw kolumn pobieramy z kluczy pierwszego wiersza
            var firstRow = data.First();
            var columns = firstRow.Keys.ToList();

            // rows -> to po prostu nasza lista data, już jest w formacie listy słowników
            // Jeżeli chcemy oddać wszystkie kolumny, to już mamy je w `data`.
            // Dla pewności: rows = data.Select(d => d.ToDictionary(kvp => kvp.Key, kvp => kvp.Value)).ToList();
            // Ale już mamy je jako słowniki, więc wystarczy data.

            var payload = new
            {
                columns = columns,
                rows = data
            };

            // 3. Wywołaj endpoint Pythonowego mikroserwisu
            var client = _httpClientFactory.CreateClient();
            var pythonUrl = "http://127.0.0.1:8000/analyze"; 

            HttpResponseMessage response;
            try
            {
                response = await client.PostAsJsonAsync(pythonUrl, payload);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error calling Python service: {ex.Message}");
            }

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                return StatusCode((int)response.StatusCode, $"Python service returned error: {errorContent}");
            }

            // 4. Odbierz wyniki analizy z Pythona
            var resultJson = await response.Content.ReadAsStringAsync();

            // Zwracamy surową odpowiedź JSON z mikroserwisu Pythonowego
            return Content(resultJson, "application/json");
        }

    }
}

