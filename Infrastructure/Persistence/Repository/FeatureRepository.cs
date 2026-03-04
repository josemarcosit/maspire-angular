using maspire_angular.Features.Feature;
using Microsoft.EntityFrameworkCore;

namespace maspire_angular.Infrastructure.Persistence.Repository
{
    public class FeatureRepository : IFeatureRepository
    {
        private readonly MaspireDbContext _context;

        public FeatureRepository(MaspireDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Feature>> GetAllFeaturesAsync(string language = null)
        {
            if (string.IsNullOrWhiteSpace(language))
            {
                return await _context.Features
                    .OrderBy(f => f.Name)
                    .ToListAsync();
            }

            // project to translated name if available
            return await _context.Features
                .Select(f => new Feature
                {
                    Id = f.Id,
                    Name = f.Translations
                             .Where(t => t.Language == language)
                             .Select(t => t.Name)
                             .FirstOrDefault() ?? f.Name
                })
                .OrderBy(f => f.Name)
                .ToListAsync();
        }

        public async Task<Feature> GetFeatureByIdAsync(int id, string language = null)
        {
            if (string.IsNullOrWhiteSpace(language))
                return await _context.Features.FindAsync(id);

            var feature = await _context.Features
                .Where(f => f.Id == id)
                .Select(f => new Feature
                {
                    Id = f.Id,
                    Name = f.Translations
                             .Where(t => t.Language == language)
                             .Select(t => t.Name)
                             .FirstOrDefault() ?? f.Name
                })
                .SingleOrDefaultAsync();

            return feature;
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
