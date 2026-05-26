using FitnessTrackerPAW.Application.DTOs;
using FitnessTrackerPAW.Domain;
using FitnessTrackerPAW.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FitnessTrackerPAW.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class ExerciseController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ExerciseController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var exercises = await _context.Exercises.ToListAsync();
            return Ok(exercises);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            var exercise = await _context.Exercises.FindAsync(id);
            if (exercise == null) return NotFound();

            return Ok(exercise);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ExerciseDto dto)
        {
            var exercise = new Exercise
            {
                Name = dto.Name,
                TargetMuscle = dto.TargetMuscle
            };

            _context.Exercises.Add(exercise);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = exercise.Id }, exercise);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ExerciseDto dto)
        {
            var exercise = await _context.Exercises.FindAsync(id);
            if (exercise == null) return NotFound();

            exercise.Name = dto.Name;
            exercise.TargetMuscle = dto.TargetMuscle;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var exercise = await _context.Exercises.FindAsync(id);
            if (exercise == null) return NotFound();

            _context.Exercises.Remove(exercise);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}