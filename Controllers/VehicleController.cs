using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using angular_vega.Controllers.Resources;
using angular_vega.Models;
using angular_vega.Persistence;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace angular_vega.Controllers
{
    [Route("/api/vehicles")]
    public class VehicleController : Controller
    {
        private readonly ILogger<VehicleController> _logger;
        private readonly IMapper mapper;
        private readonly VegaDbContext vegaDbContext;

        public VehicleController(ILogger<VehicleController> logger, IMapper mapper,VegaDbContext vegaDbContext)
        {
            _logger = logger;
            this.mapper = mapper;
            this.vegaDbContext = vegaDbContext;
        }       
     
        [HttpPost]
        public async Task<IActionResult> CreateVehicle([FromBody]VehicleResource vehicleResource){


            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }

            var model= await vegaDbContext.Models.FindAsync(vehicleResource.ModelId);

            if(model == null){
                ModelState.AddModelError("ModelId","Invalid ModelId");
                return BadRequest(ModelState);
            }
            var vehicle = mapper.Map<VehicleResource,Vehicle>(vehicleResource);
            vehicle.LasUpdate = DateTime.Now;

            vegaDbContext.Vehicles.Add(vehicle);
            await vegaDbContext.SaveChangesAsync();

            var result = mapper.Map<Vehicle,VehicleResource>(vehicle);
            return Ok(result);
        }

          [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVehicle(int id, [FromBody]VehicleResource vehicleResource){


            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }

            var model= await vegaDbContext.Models.FindAsync(vehicleResource.ModelId);

            if(model == null){
                ModelState.AddModelError("ModelId","Invalid ModelId");
                return BadRequest(ModelState);
            }
            var vehicle = await vegaDbContext.Vehicles.Include(v=> v.Features).SingleOrDefaultAsync(v=> v.Id == id);
            mapper.Map<VehicleResource,Vehicle>(vehicleResource, vehicle);
            vehicle.LasUpdate = DateTime.Now;            

            await vegaDbContext.SaveChangesAsync();

            var result = mapper.Map<Vehicle,VehicleResource>(vehicle);
            return Ok(result);
        }
          [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVehicle(int id){


            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }
            
            var vehicle = await vegaDbContext.Vehicles.FindAsync(id);            
            if(vehicle == null)
                return NotFound();
            vegaDbContext.Remove(vehicle);
            await vegaDbContext.SaveChangesAsync();

            return Ok(id);
        }

         [HttpGet("{id}")]
        public async Task<IActionResult> GetVehicle(int id){


            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }
            
            var vehicle = await vegaDbContext.Vehicles.Include(v=> v.Features).SingleOrDefaultAsync(v=> v.Id == id);             
            if(vehicle == null)
                return NotFound();
            
            var vehicleResource = mapper.Map<Vehicle,VehicleResource>(vehicle);

            return Ok(vehicleResource);
        }
    }
}