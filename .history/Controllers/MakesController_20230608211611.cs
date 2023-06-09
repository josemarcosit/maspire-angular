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
    // [Route("[controller]")]
    public class MakesController : Controller
    {
        private readonly ILogger<MakesController> _logger;
        private readonly VegaDbContext vegaDbContext;
        private readonly IMapper mapper;

        public MakesController(ILogger<MakesController> logger, VegaDbContext vegaDbContext, IMapper mapper)
        {
            _logger = logger;
            this.vegaDbContext = vegaDbContext;
            this.mapper = mapper;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("/api/makes")]
        public async Task<IEnumerable<MakeResource>> GetMakes(){
            
            var makes = await vegaDbContext.Makes
                .Include(m => m.Models)
                .ToListAsync();
                
                return await mapper.Map<List<Make>,List<MakeResource>>(makes).ToListAsync();    
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}