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
    public class FeaturesController : Controller
    {
        private readonly ILogger<FeaturesController> _logger;
        private readonly VegaDbContext vegaDbContext;
        private readonly IMapper mapper;
        public FeaturesController(ILogger<FeaturesController> logger, VegaDbContext vegaDbContext, IMapper mapper)
        {
            _logger = logger;
            this.vegaDbContext = vegaDbContext;
            this.mapper = mapper;
        }

        public IActionResult Index()
        {
            return View();
        }
        
        [HttpGet("/api/vehicle/features")]
        public async Task<IEnumerable<Feature>> GetFeatures(){
            
            return  await vegaDbContext.Features.ToListAsync();
                
                
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}