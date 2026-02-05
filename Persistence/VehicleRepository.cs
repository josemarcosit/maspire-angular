using System.Linq.Expressions;
using angular_vega.Core.Models;
using angular_vega.Extensions;
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
            if (!includeRelated)
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

        public async Task<IEnumerable<Vehicle>> GetVehicles(VehicleQuery queryObj)
        {
            var query = vegaDbContext.Vehicles
             .Include(v => v.Model)
             .ThenInclude(m => m.Make)
             .Include(v => v.Features)
             .ThenInclude(vf => vf.Feature)
             .AsQueryable();

            if (queryObj.MakeId.HasValue)
                query = query.Where(v => v.Model.MakeId == queryObj.MakeId);

            if (queryObj.ModelId.HasValue)
                query = query.Where(v => v.ModelId == queryObj.ModelId);

            if (queryObj.SortBy == "make")
                query = queryObj.IsSortAscending ? query.OrderBy(v => v.Model.Make.Name) : query.OrderByDescending(v => v.Model.Make.Name);

            if (queryObj.SortBy == "model")
                query = queryObj.IsSortAscending ? query.OrderBy(v => v.Model.Name) : query.OrderByDescending(v => v.Model.Name);

             if (queryObj.SortBy == "contactName")
                query = queryObj.IsSortAscending ? query.OrderBy(v => v.ContactName) : query.OrderByDescending(v => v.ContactName);

            if (queryObj.SortBy == "id")
                query = queryObj.IsSortAscending ? query.OrderBy(v => v.Id) : query.OrderByDescending(v => v.Id);

            var columnsMap = new Dictionary<string, Expression<Func<Vehicle, object>>>()
            {
                ["make"] = v => v.Model.Make.Name,
                ["model"] = v => v.Model.Name,
                ["contactName"] = v => v.ContactName              
            };

            query = query.ApplyOrdering(queryObj, columnsMap);
            
            query = query.ApplySorting(queryObj);

            return await query.ToListAsync();
        }
    }
}