using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using angular_vega.Models;
using angular_vega.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace angular_vega.Controllers
{
    [Route("[controller]")]
    public class MakesController : Controller
    {
        private readonly ILogger<MakesController> _logger;
        private readonly VegaDbContext vegaDbContext;

        public MakesController(ILogger<MakesController> logger, VegaDbContext vegaDbContext)
        {
            _logger = logger;
            this.vegaDbContext = vegaDbContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("/api/makes")]
        public IEnumerable<Make> GetMakes(){
            return vegaDbContext.Makes
                .Include(m => m.Models)
                .ToList();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}