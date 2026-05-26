using FitnessTrackerPAW.Domain;

namespace FitnessTrackerPAW.Application.Interfaces
{
    public interface ISupplementRepository
    {
        Task<IEnumerable<Supplement>> GetAllAsync();
        Task AddAsync(Supplement supplement);
        Task SaveChangesAsync();
    }
}