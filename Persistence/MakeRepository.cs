using angular_vega.Core;
using angular_vega.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace angular_vega.Persistence
{
    public class MakeRepository : IMakeRepository
    {
        private readonly VegaDbContext _context;

        public MakeRepository(VegaDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Make>> GetAllMakesAsync()
        {
            return await _context.Makes                    
            .ToListAsync();           
        }
    }
}
