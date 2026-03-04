namespace maspire_angular.Features.Make
{
    public interface IMakeRepository
    {
        Task<IEnumerable<Make>> GetAllMakesAsync();
    }
}
