using angular_vega.Core.Models;

namespace angular_vega.Core
{
    public interface IFeatureRepository
    {
        /// <summary>
        /// Retrieves all features. If <paramref name="language"/> is provided the
        /// repository will attempt to return the translated name for each feature.
        /// </summary>
        Task<IEnumerable<Feature>> GetAllFeaturesAsync(string language = null);

        /// <summary>
        /// Retrieves a single feature by id. A language parameter will return the
        /// translated name when available.
        /// </summary>
        Task<Feature> GetFeatureByIdAsync(int id, string language = null);

        Task AddFeatureAsync(Feature feature);
        Task UpdateFeatureAsync(Feature feature);
        Task DeleteFeatureAsync(int id);
    }
}
