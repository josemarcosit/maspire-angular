namespace maspire_angular.Features.Feature
{
    public interface IFeatureRepository
    {
        Task<IEnumerable<Feature>> GetAllFeaturesAsync(string language = null);
        Task<Feature> GetFeatureByIdAsync(int id, string language = null);
        Task AddFeatureAsync(Feature feature);
        Task UpdateFeatureAsync(Feature feature);
        Task DeleteFeatureAsync(int id);
    }
}
