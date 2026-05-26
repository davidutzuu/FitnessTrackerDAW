using FitnessTrackerPAW.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FitnessTrackerPAW.Controllers
{
    [Route("api/admin")]
    [ApiController]
    // AICI ESTE MAGIA: Doar conturile cu "stampila" de Admin pot accesa acest controller
    [Authorize(Roles = "Admin")]
    public class AdminApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AdminApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. ENDPOINT: GET /api/admin/users
        // Returneaza lista tuturor conturilor inregistrate in aplicatie
        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            // Extragem doar ID-ul si Email-ul pentru a nu expune parolele hash-uite
            var users = await _context.Users
                .Select(u => new { u.Id, u.Email })
                .ToListAsync();

            return Ok(users);
        }

        // 2. ENDPOINT: DELETE /api/admin/supplements/{id}
        // Permite adminului sa stearga definitiv orice supliment
        [HttpDelete("supplements/{id}")]
        public async Task<IActionResult> DeleteSupplement(int id)
        {
            var supplement = await _context.Supplements.FindAsync(id);
            if (supplement == null)
            {
                return NotFound(new { Message = "Suplimentul nu a fost gasit." });
            }

            _context.Supplements.Remove(supplement);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Suplimentul a fost sters definitiv din sistem!" });
        }
    }
}