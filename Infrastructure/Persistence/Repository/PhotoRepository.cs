using maspire_angular.Features.Photo;
using Microsoft.EntityFrameworkCore;

namespace maspire_angular.Infrastructure.Persistence.Repository
{
    public class PhotoRepository : IPhotoRepository
    {
        private readonly MaspireDbContext maspireDbContext;
        public PhotoRepository(MaspireDbContext maspireDbContext)
        {
            this.maspireDbContext = maspireDbContext;
        }
        public async Task<IEnumerable<Photo>> GetPhotosByVehicleId(int vehicleId)
        {
            return await maspireDbContext.Photos
            .Where(p => p.VehicleId == vehicleId)
            .ToListAsync();
        }
    }
}