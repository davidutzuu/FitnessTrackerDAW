using FitnessTrackerPAW.Application.DTOs;
using FitnessTrackerPAW.Application.Interfaces;
using FitnessTrackerPAW.Domain;

namespace FitnessTrackerPAW.Application.Services
{
    public class SupplementService : ISupplementService
    {
        private readonly ISupplementRepository _repository;
        private readonly ILogger<SupplementService> _logger;

        public SupplementService(ISupplementRepository repository, ILogger<SupplementService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<IEnumerable<SupplementDto>> GetAllSupplementsAsync()
        {
            _logger.LogInformation("Se preiau toate suplimentele din baza de date.");
            var supplements = await _repository.GetAllAsync();

            // Mapare manuala din Entitate in DTO
            return supplements.Select(s => new SupplementDto
            {
                Name = s.Name,
                CaloriesPerServing = s.CaloriesPerServing,
                ProteinPerServing = s.ProteinPerServing,
                IsMassGainer = s.IsMassGainer,
                UserId = s.UserId.ToString() // <--- Aici este maparea curierului!
            });
        }

        public async Task AddSupplementAsync(SupplementDto dto, string userId)
        {
            _logger.LogInformation($"Adaugare supliment nou: {dto.Name} pentru user-ul {userId}");

            var supplement = new Supplement
            {
                Name = dto.Name,
                CaloriesPerServing = dto.CaloriesPerServing,
                ProteinPerServing = dto.ProteinPerServing,
                IsMassGainer = dto.IsMassGainer,
                DateConsumed = DateTime.UtcNow,
                UserId = userId
            };

            await _repository.AddAsync(supplement);
            await _repository.SaveChangesAsync();
        }

        public async Task ResetAllSupplementsAsync(string userId)
        {
            _logger.LogInformation($"Reset toate suplimentele pentru user-ul {userId}");
            // ↑ Logheza operatia de reset

            var allSupplements = await _repository.GetAllAsync();
            // ↑ Aduceti TOTI suplimentele din DB

            var userSupplements = allSupplements.Where(s => s.UserId == userId).ToList();
            // ↑ Filtreaza - numai suplimentele utilizatorului curent

            foreach (var supplement in userSupplements)
            {
                _repository.DeleteAsync(supplement.Id);
                // ↑ Sterge fiecare supliment al utilizatorului
            }

            await _repository.SaveChangesAsync();
            // ↑ Salveaza changurile in DB (DELETE queries)
        }
    }
}