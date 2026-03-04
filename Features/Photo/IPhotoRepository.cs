namespace maspire_angular.Features.Photo
{
    public interface IPhotoRepository
    {
        Task<IEnumerable<Photo>> GetPhotosByVehicleId(int vehicleId);
    }
}
