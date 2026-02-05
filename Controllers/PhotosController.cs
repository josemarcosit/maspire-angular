using angular_vega.Controllers.Resources;
using angular_vega.Core;
using angular_vega.Core.Models;
using angular_vega.Persistence;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace angular_vega.Controllers
{
    [Route("/api/vehicles/{vehicleId}/photos")]
    public class PhotosController : Controller
    {
        private const int MAX_BYTES = 10 * 1024 * 1024;
        private readonly string[] ACCEPTED_FILE_TYPES = new[] { ".png" };
        private readonly IHostEnvironment _host;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public PhotosController(IHostEnvironment host,
                                IVehicleRepository vehicleRepository,
                                IUnitOfWork unitOfWork,
                                IMapper mapper)
        {
            _host = host;
            _vehicleRepository = vehicleRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> Upload(int vehicleId, IFormFile file)
        {
            var vehicle = await _vehicleRepository.GetVehicle(vehicleId, false);

            if (vehicle == null) return NotFound();

            if (file == null) return BadRequest("Null file");

            if (file.Length == 0) return BadRequest("Empty file");

            if (file.Length > MAX_BYTES) return BadRequest("Maximun file size exceeded");

            if (!ACCEPTED_FILE_TYPES.Any(p => p == Path.GetExtension(file.FileName)))          
                return BadRequest("Invalid file type");            

            var uploadsFolderPtah = Path.Combine(_host.ContentRootPath, "uploads");

            if (!Directory.Exists(uploadsFolderPtah))            
                Directory.CreateDirectory(uploadsFolderPtah);            

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadsFolderPtah, fileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);

            var photo = new Photo() { FileName = fileName };
            vehicle.Photos.Add(photo);

            await _unitOfWork.CompleteAsync();

            return Ok(_mapper.Map<Photo, PhotoResource>(photo));
        }
    }
}
