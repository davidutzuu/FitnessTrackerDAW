using FitnessTrackerPAW.Application.DTOs;
using FitnessTrackerPAW.Application.Interfaces;
using FitnessTrackerPAW.Domain;

namespace FitnessTrackerPAW.Application.Services
{
    public class ExerciseService : IExerciseService
    {
        private readonly IExerciseRepository _repository;
        private readonly ILogger<ExerciseService> _logger;

        public ExerciseService(IExerciseRepository repository, ILogger<ExerciseService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<IEnumerable<ExerciseDto>> GetAllAsync()
        {
            _logger.LogInformation("Se preiau toate exercitiile din baza de date.");
            var exercises = await _repository.GetAllAsync();
            return exercises.Select(e => new ExerciseDto
            {
                Id = e.Id,
                Name = e.Name,
                TargetMuscle = e.TargetMuscle
            });
        }

        public async Task<ExerciseDto?> GetByIdAsync(int id)
        {
            _logger.LogInformation("Se cauta exercitiul cu id={Id}.", id);
            var exercise = await _repository.GetByIdAsync(id);
            if (exercise == null)
            {
                _logger.LogWarning("Exercitiul cu id={Id} nu a fost gasit.", id);
                return null;
            }
            return new ExerciseDto { Id = exercise.Id, Name = exercise.Name, TargetMuscle = exercise.TargetMuscle };
        }

        public async Task<ExerciseDto> CreateAsync(ExerciseDto dto)
        {
            _logger.LogInformation("Adaugare exercitiu nou: {Name}.", dto.Name);
            try
            {
                var exercise = new Exercise { Name = dto.Name, TargetMuscle = dto.TargetMuscle };
                await _repository.AddAsync(exercise);
                await _repository.SaveChangesAsync();
                dto.Id = exercise.Id;
                return dto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Eroare la adaugarea exercitiului: {Name}.", dto.Name);
                throw;
            }
        }

        public async Task<bool> UpdateAsync(int id, ExerciseDto dto)
        {
            _logger.LogInformation("Actualizare exercitiu cu id={Id}.", id);
            var exercise = await _repository.GetByIdAsync(id);
            if (exercise == null)
            {
                _logger.LogWarning("Exercitiul cu id={Id} nu a fost gasit pentru actualizare.", id);
                return false;
            }
            try
            {
                exercise.Name = dto.Name;
                exercise.TargetMuscle = dto.TargetMuscle;
                await _repository.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Eroare la actualizarea exercitiului cu id={Id}.", id);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            _logger.LogInformation("Stergere exercitiu cu id={Id}.", id);
            var exercise = await _repository.GetByIdAsync(id);
            if (exercise == null)
            {
                _logger.LogWarning("Exercitiul cu id={Id} nu a fost gasit pentru stergere.", id);
                return false;
            }
            try
            {
                _repository.Delete(exercise);
                await _repository.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Eroare la stergerea exercitiului cu id={Id}.", id);
                throw;
            }
        }
    }
}
