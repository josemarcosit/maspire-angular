using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using angular_vega.Controllers.Resources;
using angular_vega.Core;
using angular_vega.Core.Models;
using angular_vega.Core.Models;
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
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IUnitOfWork unitOfWork;

        public VehicleController(ILogger<VehicleController> logger, IMapper mapper,        
        IVehicleRepository vehicleRepository,
        IUnitOfWork unitOfWork)
        {
            _vehicleRepository = vehicleRepository;
            this.unitOfWork = unitOfWork;
            _logger = logger;
            this.mapper = mapper;            
        }

        [HttpPost]
        public async Task<IActionResult> CreateVehicle([FromBody] SaveVehicleResource vehicleResource)
        {
            //throw new Exception();
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var vehicle = mapper.Map<SaveVehicleResource, Vehicle>(vehicleResource);
            vehicle.LasUpdate = DateTime.Now;

            _vehicleRepository.Add(vehicle);
            await unitOfWork.CompleteAsync();

            vehicle = await _vehicleRepository.GetVehicle(vehicle.Id);

            var result = mapper.Map<Vehicle, VehicleResource>(vehicle);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVehicle(int id, [FromBody] SaveVehicleResource vehicleResource)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var vehicle = await _vehicleRepository.GetVehicle(id);

            if (vehicle == null)
            {
                return NotFound();
            }
            mapper.Map<SaveVehicleResource, Vehicle>(vehicleResource, vehicle);
            vehicle.LasUpdate = DateTime.Now;

            await unitOfWork.CompleteAsync();

            var result = mapper.Map<Vehicle, VehicleResource>(vehicle);
            return Ok(result);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVehicle(int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            
            var vehicle = await _vehicleRepository.GetVehicle(id);
            if (vehicle == null)
                return NotFound();

            _vehicleRepository.Remove(vehicle);
            await unitOfWork.CompleteAsync();

            return Ok(id);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetVehicle(int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var vehicle = await _vehicleRepository.GetVehicle(id);

            if (vehicle == null)
                return NotFound();

            var vehicleResource = mapper.Map<Vehicle, VehicleResource>(vehicle);

            return Ok(vehicleResource);
        }
        
        [HttpGet]
        public async Task<IActionResult> GetVehicles(VehicleQueryResource filterResourse)
        {           
            var filter = mapper.Map<VehicleQueryResource,VehicleQuery>(filterResourse);

            var vehicles = await _vehicleRepository.GetVehicles(filter);      

            var vehicleResource = mapper.Map<IEnumerable<Vehicle>, IEnumerable<VehicleResource>>(vehicles);

            return Ok(vehicleResource);
        }
    }
}