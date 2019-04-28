using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IPCViewer.Api.Models;

namespace IPCViewer.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitiesController : ControllerBase
    {
        private readonly ICityRepository cityRepository;

        public CitiesController(ICityRepository cityRepository)
        {
            this.cityRepository = cityRepository;
        }

        // GET: api/Cities
        [HttpGet]
        public IActionResult GetCities()
        {
            return Ok(this.cityRepository.GetAll());
        }


        // GET: api/Cities/5
        
    }
}
