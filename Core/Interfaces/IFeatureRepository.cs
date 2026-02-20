using angular_vega.Core.Models;

namespace angular_vega.Core
{
    public interface IFeatureRepository
    {
        Task<IEnumerable<Feature>> GetAllFeaturesAsync();
        Task<Feature> GetFeatureByIdAsync(int id);
        Task AddFeatureAsync(Feature feature);
        Task UpdateFeatureAsync(Feature feature);
        Task DeleteFeatureAsync(int id);
    }
}
