using FitnessTrackerPAW.Application.DTOs;
using FitnessTrackerPAW.Domain;
using FitnessTrackerPAW.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FitnessTrackerPAW.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WorkoutSessionController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public WorkoutSessionController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] WorkoutSessionDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Forbid();

            var session = new WorkoutSession
            {
                Date = DateTime.UtcNow,
                DurationInMinutes = dto.DurationInMinutes,
                PumpLevel = dto.PumpLevel,
                UserId = userId
            };

            _context.WorkoutSessions.Add(session);
            await _context.SaveChangesAsync();

            return StatusCode(201, new { Message = "Antrenament salvat cu succes!" });
        }
    }
}