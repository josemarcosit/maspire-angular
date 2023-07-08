using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using angular_vega.Core;
using angular_vega.Core.Models;

namespace angular_vega.Persistence
{
    public interface IVehicleRepository
    {
        Task<Vehicle> GetVehicle(int id, bool includeRelated = true);
        Task<Vehicle> GetVehicleWithMake(int id);
        void Add(Vehicle vehicle);
        void Remove(Vehicle vehicle);
        Task<IEnumerable<Vehicle>> GetVehicles(VehicleQuery vehicleQuery);
    }
}