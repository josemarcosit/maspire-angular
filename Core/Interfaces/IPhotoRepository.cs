using angular_vega.Core.Models;

namespace angular_vega.Core
{
    public interface IPhotoRepository
    {
       Task<IEnumerable<Photo>> GetPhotosByVehicleId(int vehicleId);
    }
}
