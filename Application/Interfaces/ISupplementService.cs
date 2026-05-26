using FitnessTrackerPAW.Application.DTOs;

namespace FitnessTrackerPAW.Application.Interfaces
{
    public interface ISupplementService
    {
        Task<IEnumerable<SupplementDto>> GetAllSupplementsAsync();
        Task AddSupplementAsync(SupplementDto supplementDto, string userId);
    }
}