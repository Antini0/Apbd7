using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace apbd7;


    [ApiController]
    [Route("[controller]")]
    public class WarehouseController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public WarehouseController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> AddProductToWarehouse(ProductWarehouseRequest request)
        {
            // Walidacja żądania
            if (request == null)
            {
                return BadRequest("Nieprawidłowe dane żądania.");
            }

            // Walidacja danych
            if (request.IdProduct <= 0 || request.IdWarehouse <= 0 || request.Ammount <= 0)
            {
                return BadRequest("Identyfikatory produktu i magazynu oraz ilość muszą być większe niż zero.");
            }

            // Połączenie z bazą danych
            using (var connection = new SqlConnection(_configuration.GetConnectionString("YourConnectionString")))
            {
                await connection.OpenAsync();

                // Tworzenie i wykonanie zapytania SQL
                var query = @"
                    -- Twoje zapytanie SQL w celu dodania produktu do magazynu
                    -- Pamiętaj, aby wykorzystać parametry zabezpieczone, aby uniknąć ataków SQL Injection
                ";

                using (var command = new SqlCommand(query, connection))
                {
                    // Przekazanie parametrów do zapytania SQL
                    command.Parameters.AddWithValue("@IdProduct", request.IdProduct);
                    command.Parameters.AddWithValue("@IdWarehouse", request.IdWarehouse);
                    command.Parameters.AddWithValue("@Ammount", request.Ammount);
                    command.Parameters.AddWithValue("@CreatedAt", request.CreatedAt);

                    // Wykonanie zapytania i zwrócenie odpowiedzi
                    try
                    {
                        await command.ExecuteNonQueryAsync();
                        return Ok("Produkt został dodany do magazynu.");
                    }
                    catch (Exception ex)
                    {
                        return StatusCode(500, $"Wystąpił błąd podczas dodawania produktu do magazynu: {ex.Message}");
                    }
                }
            }
        }
    }

    public class ProductWarehouseRequest
    {
        public int IdProduct { get; set; }
        public int IdWarehouse { get; set; }
        public int Ammount { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}