using FitnessTrackerPAW.Application.DTOs;

namespace FitnessTrackerPAW.Application.Interfaces
{
    public interface IExerciseService
    {
        Task<IEnumerable<ExerciseDto>> GetAllAsync();
        Task<ExerciseDto?> GetByIdAsync(int id);
        Task<ExerciseDto> CreateAsync(ExerciseDto dto);
        Task<bool> UpdateAsync(int id, ExerciseDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
