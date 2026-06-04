using FitnessTrackerPAW.Domain;

namespace FitnessTrackerPAW.Application.Interfaces
{
    public interface ISupplementRepository
    {
        Task<IEnumerable<Supplement>> GetAllAsync();
        Task AddAsync(Supplement supplement);
        void DeleteAsync(int supplementId);
        // ↑ NOUA METODA: Sterge supliment dupa ID
        Task SaveChangesAsync();
    }
}