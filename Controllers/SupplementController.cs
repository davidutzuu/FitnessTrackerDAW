using FitnessTrackerPAW.Application.DTOs;
using FitnessTrackerPAW.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FitnessTrackerPAW.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // 401 Unauthorized daca userul nu este logat
    public class SupplementController : ControllerBase
    {
        private readonly ISupplementService _service;

        public SupplementController(ISupplementService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var supplements = await _service.GetAllSupplementsAsync();
            return Ok(supplements); // 200 OK
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SupplementDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // 400 Bad Request
            }

            // Extragem ID-ul userului din token-ul de autentificare
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Forbid(); // 403 Forbidden

            await _service.AddSupplementAsync(dto, userId);

            // Returnam 201 Created cand o resursa noua a fost facuta cu succes
            return StatusCode(201, new { Message = "Supliment adaugat cu succes!" });
        }

        [HttpDelete("reset")]
        // ↑ DELETE /api/supplement/reset
        // Sterge TOTI suplimentele utilizatorului logat
        public async Task<IActionResult> ResetSupplements()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Forbid(); // 403 Forbidden

            await _service.ResetAllSupplementsAsync(userId);

            return NoContent(); // 204 No Content
        }
    }
}