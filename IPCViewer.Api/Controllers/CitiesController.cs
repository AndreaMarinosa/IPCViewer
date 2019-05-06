﻿namespace IPCViewer.Api.Controllers
{
    using Data;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiController]
    public class CitiesController : Controller
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
        [HttpGet]
        public IActionResult GetCities(int id)
        {
            return Ok(this.cityRepository.GetCityByIdAsync(id));
        }
    }
}
