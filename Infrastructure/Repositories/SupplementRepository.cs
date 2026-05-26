using FitnessTrackerPAW.Application.Interfaces;
using FitnessTrackerPAW.Domain;
using Microsoft.EntityFrameworkCore;

namespace FitnessTrackerPAW.Infrastructure.Repositories
{
    public class SupplementRepository : ISupplementRepository
    {
        private readonly ApplicationDbContext _context;

        public SupplementRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Supplement>> GetAllAsync()
        {
            return await _context.Supplements.ToListAsync();
        }

        public async Task AddAsync(Supplement supplement)
        {
            await _context.Supplements.AddAsync(supplement);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}