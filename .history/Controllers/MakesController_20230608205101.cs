using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using angular_vega.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace angular_vega.Controllers
{
    [Route("[controller]")]
    public class MakesController : Controller
    {
        private readonly ILogger<MakesController> _logger;

        public MakesController(ILogger<MakesController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("/api/makes")]
        public IEnumerable<Make> GetMakes(){
            
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}