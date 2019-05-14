namespace IPCViewer.Api.Controllers
{
    using Data;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiController]
    public class CitiesController : ControllerBase
    {
        private readonly ICityRepository cityRepository;

        public CitiesController (ICityRepository cityRepository)
        {
            this.cityRepository = cityRepository;
        }

        // GET: api/Cities
        [HttpGet]
        public IActionResult GetCities ()
        {
            return Ok(this.cityRepository.GetAll());
        }

    }
}