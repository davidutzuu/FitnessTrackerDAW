using FitnessTrackerPAW.Domain;

namespace FitnessTrackerPAW.Application.Interfaces
{
    public interface IExerciseRepository
    {
        Task<IEnumerable<Exercise>> GetAllAsync();
        Task<Exercise?> GetByIdAsync(int id);
        Task AddAsync(Exercise exercise);
        void Delete(Exercise exercise);
        Task SaveChangesAsync();
    }
}
