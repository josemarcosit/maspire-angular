using maspire_angular.Features.Make;
using Microsoft.EntityFrameworkCore;

namespace maspire_angular.Infrastructure.Persistence.Repository
{
    public class MakeRepository : IMakeRepository
    {
        private readonly MaspireDbContext _context;

        public MakeRepository(MaspireDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Make>> GetAllMakesAsync()
        {
            return await _context.Makes
            .Include(m => m.Models)
            .ToListAsync();
        }
    }
}
