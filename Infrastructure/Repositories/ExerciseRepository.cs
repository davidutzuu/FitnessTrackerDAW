using FitnessTrackerPAW.Application.Interfaces;
using FitnessTrackerPAW.Domain;
using Microsoft.EntityFrameworkCore;

namespace FitnessTrackerPAW.Infrastructure.Repositories
{
    public class ExerciseRepository : IExerciseRepository
    {
        private readonly ApplicationDbContext _context;

        public ExerciseRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Exercise>> GetAllAsync()
        {
            return await _context.Exercises.ToListAsync();
        }

        public async Task<Exercise?> GetByIdAsync(int id)
        {
            return await _context.Exercises.FindAsync(id);
        }

        public async Task AddAsync(Exercise exercise)
        {
            await _context.Exercises.AddAsync(exercise);
        }

        public void Delete(Exercise exercise)
        {
            _context.Exercises.Remove(exercise);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
