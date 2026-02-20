using angular_vega.Core;
using angular_vega.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace angular_vega.Persistence
{
    public class FeatureRepository : IFeatureRepository
    {
        private readonly VegaDbContext _context;

        public FeatureRepository(VegaDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Feature>> GetAllFeaturesAsync()
        {
            return await _context.Features
                .OrderBy(f => f.Name)
                .ToListAsync();
        }

        public async Task<Feature> GetFeatureByIdAsync(int id)
        {
            return await _context.Features.FindAsync(id);
        }

        public async Task AddFeatureAsync(Feature feature)
        {
            _context.Features.Add(feature);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateFeatureAsync(Feature feature)
        {
            _context.Features.Update(feature);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteFeatureAsync(int id)
        {
            var feature = await GetFeatureByIdAsync(id);
            if (feature != null)
            {
                _context.Features.Remove(feature);
                await _context.SaveChangesAsync();
            }
        }
    }
}
