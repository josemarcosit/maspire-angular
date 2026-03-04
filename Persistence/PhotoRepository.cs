using angular_vega.Core;
using angular_vega.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace angular_vega.Persistence
{
    public class PhotoRepository : IPhotoRepository
    {
        private readonly VegaDbContext vegaDbContext;
        public PhotoRepository(VegaDbContext vegaDbContext)
        {
            this.vegaDbContext = vegaDbContext;
        }
        public async Task<IEnumerable<Photo>> GetPhotosByVehicleId(int vehicleId)
        {
            return await vegaDbContext.Photos
            .Where(p => p.VehicleId == vehicleId)
            .ToListAsync();
        }
    }
}