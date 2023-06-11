using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using angular_vega.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace angular_vega.Persistence
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly VegaDbContext vegaDbContext;

        public VehicleRepository(VegaDbContext vegaDbContext)
        {
            this.vegaDbContext = vegaDbContext;
        }

        public void Add(Vehicle vehicle)
        {
            vegaDbContext.Add(vehicle);
        }

         public void Remove(Vehicle vehicle)
        {
            vegaDbContext.Remove(vehicle);
        }

        public async Task<Vehicle> GetVehicle(int id, bool includeRelated = true)
        {
            if(!includeRelated)
                return await vegaDbContext.Vehicles.FindAsync(id);

            return await vegaDbContext.Vehicles
              .Include(v => v.Features)
              .ThenInclude(vf => vf.Feature)
              .Include(v => v.Model)
              .Include(v => v.Model.Make)
              .SingleOrDefaultAsync(v => v.Id == id);
        }

        public async Task<Vehicle> GetVehicleWithMake(int id)
        {
             return await vegaDbContext.Vehicles
              .Include(v => v.Features)
              .ThenInclude(vf => vf.Feature)
              .Include(v => v.Model)
              .Include(v => v.Model.Make)
              .SingleOrDefaultAsync(v => v.Id == id);
        }

          public async Task<IEnumerable<Vehicle>> GetVehicles()
        {
             return await vegaDbContext.Vehicles
              .Include(v => v.Features)
              .ThenInclude(vf => vf.Feature)
              .Include(v => v.Model)
              .Include(v => v.Model.Make)
              .ToListAsync();
        }
    }
}