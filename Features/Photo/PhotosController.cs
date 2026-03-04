using AutoMapper;
using maspire_angular.Features.Vehicle;
using maspire_angular.Shared.Abstractions;
using maspire_angular.Shared.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace maspire_angular.Features.Photo
{
    [Route("/api/vehicles/{vehicleId}/photos")]
    [Authorize]
    public class PhotosController : Controller
    {
        private const int MAX_BYTES = 10 * 1024 * 1024;
        private readonly string[] ACCEPTED_FILE_TYPES = new[] { ".png" };
        private readonly IHostEnvironment _host;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IPhotoRepository _photoRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _localizer;

        public PhotosController(IHostEnvironment host,
                                IVehicleRepository vehicleRepository,
                                IPhotoRepository photoRepository,
                                IUnitOfWork unitOfWork,
                                IMapper mapper,
                                IStringLocalizer<SharedResources> localizer)
        {
            _host = host;
            _vehicleRepository = vehicleRepository;
            _photoRepository = photoRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizer = localizer;
        }

        [HttpPost]
        public async Task<IActionResult> Upload(int vehicleId, IFormFile file)
        {
            var vehicle = await _vehicleRepository.GetVehicle(vehicleId, false);

            if (vehicle == null) return NotFound();

            if (file == null) return BadRequest(_localizer["NullFile"]);

            if (file.Length == 0) return BadRequest(_localizer["EmptyFile"]);

            if (file.Length > MAX_BYTES) return BadRequest(_localizer["FileMaxSizeExceeded"]);

            if (!ACCEPTED_FILE_TYPES.Any(p => p == Path.GetExtension(file.FileName)))
                return BadRequest(_localizer["InvalidFileType"]);

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

        [HttpGet]
        public async Task<IActionResult> GetPhotos(int vehicleId)
        {
            var photos = await _photoRepository.GetPhotosByVehicleId(vehicleId);
            return Ok(_mapper.Map<IEnumerable<Photo>, IEnumerable<PhotoResource>>(photos));
        }
    }
}
