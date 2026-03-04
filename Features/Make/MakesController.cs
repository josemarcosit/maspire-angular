using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace maspire_angular.Features.Make
{
    [Route("[controller]")]
    [Authorize]
    public class MakesController : Controller
    {
        private readonly ILogger<MakesController> _logger;
        private readonly IMakeRepository _makeRepository;
        private readonly IMapper mapper;
        public MakesController(ILogger<MakesController> logger,
                               IMapper mapper,
                               IMakeRepository makeRepository)
        {
            _logger = logger;
            this.mapper = mapper;
            _makeRepository = makeRepository;
        }

        [HttpGet("/api/makes")]
        public async Task<IActionResult> GetMakes()
        {
            var makes = await _makeRepository.GetAllMakesAsync();

            return Ok(mapper.Map<IEnumerable<Make>, IEnumerable<MakeResource>>(makes));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}