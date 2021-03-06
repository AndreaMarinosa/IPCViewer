﻿namespace IPCViewer.Api.Controllers
{
    using IPCViewer.Api.Data;
    using IPCViewer.Api.Helpers;
    using IPCViewer.Api.Models;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.IO;
    using System.Threading.Tasks;

    [Route("api/[controller]")]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class CamerasController : Controller
    {
        private readonly ICameraRepository cameraRepository;
        private readonly ICityRepository cityRespository;
        private readonly IUserHelper userHelper;

        public CamerasController (
            ICameraRepository cameraRepository,
            IUserHelper userHelper,
            ICityRepository cityRespository)
        {
            this.cameraRepository = cameraRepository;
            this.userHelper = userHelper;
            this.cityRespository = cityRespository;
        }

        // GET: api/Cameras
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult GetCameras () => Ok(cameraRepository.GetAllWithUsers());

        // GET: api/Cameras/5
        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult GetCamera (int id) => Ok(cameraRepository.GetCamera(id).Result);

        //PUT
        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> PutCamera ([FromRoute] int id, [FromBody] Common.Models.Camera camera)
        {
            if ( !ModelState.IsValid )
            {
                return this.BadRequest(ModelState);
            }

            if ( id != camera.Id )
            {
                return BadRequest();
            }

            var oldCamera = await this.cameraRepository.GetByIdAsync(id);
            if ( oldCamera == null )
            {
                return this.BadRequest("Camera Id don't exists.");
            }

            var city = await this.cityRespository.GetCityByIdAsync(camera.CityId);
            if ( city == null )
            {
                return this.BadRequest("Invalid city");
            }

            // Image part
            var imageUrl = string.Empty;

            // Si tiene la camara
            if ( camera.ImageArray != null && camera.ImageArray.Length > 0 )
            {
                var stream = new MemoryStream(camera.ImageArray);
                var guid = Guid.NewGuid().ToString();
                var file = $"{guid}.jpg";
                var folder = "wwwroot\\images\\cameras";
                var fullPath = $"{file}";
                var response = FilesHelper.UploadPhoto(stream, folder, file);

                // si puede subir la imagen
                if ( response )
                {
                    imageUrl = fullPath;
                }
            }
            else
            {
                imageUrl = camera.ImageUrl;
            }

            oldCamera.ImageUrl = imageUrl;
            oldCamera.Comments = camera.Comments;
            oldCamera.Name = camera.Name;
            oldCamera.Latitude = camera.Latitude;
            oldCamera.Longitude = camera.Longitude;
            oldCamera.City = city;
            oldCamera.CityId = city.Id;

            var updatedCamera = await this.cameraRepository.UpdateAsync(oldCamera);
            return Ok(updatedCamera);
        }

        // POST Camera
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> PostCamera ([FromBody] Common.Models.Camera camera)
        {
            if ( !ModelState.IsValid )
            {
                return this.BadRequest(ModelState);
            }

            var user = await this.userHelper.GetUserByEmailAsync(camera.User.Email);
            if ( user == null )
            {
                return this.BadRequest("Invalid user");
            }

            var city = await this.cityRespository.GetCityByIdAsync(camera.CityId);
            if ( city == null )
            {
                return this.BadRequest("Invalid city");
            }

            // Image part
            var imageUrl = string.Empty;
            if ( camera.ImageArray != null && camera.ImageArray.Length > 0 )
            {
                var stream = new MemoryStream(camera.ImageArray);
                var guid = Guid.NewGuid().ToString();
                var file = $"{guid}.jpg";
                var folder = "wwwroot\\images\\cameras";
                var fullPath = $"{file}";
                var response = FilesHelper.UploadPhoto(stream, folder, file);

                // si puede subir la imagen
                if ( response )
                {
                    imageUrl = fullPath;
                }
            }
            else
            {
                imageUrl = camera.ImageUrl;
            }

            var entityCamera = new Camera
            {
                Name = camera.Name,
                Comments = camera.Comments,
                CreatedDate = DateTime.Now,
                Latitude = camera.Latitude,
                Longitude = camera.Longitude,
                User = user,
                CityId = camera.CityId,
                City = city,
                ImageUrl = imageUrl
            };

            var newCamera = await cameraRepository.CreateAsync(entityCamera);
            return Ok(newCamera);
        }

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeleteCamera ([FromRoute] int id)
        {
            if ( !ModelState.IsValid )
            {
                return this.BadRequest(ModelState);
            }

            var camera = await this.cameraRepository.GetByIdAsync(id);
            if ( camera == null )
            {
                return this.NotFound();
            }

            await this.cameraRepository.DeleteAsync(camera);
            return Ok(camera);
        }

        // GET: api/Cameras/Image/
        [HttpGet]
        [Route("Image/{name}")]
        [AllowAnonymous]
        public IActionResult GetImage([FromRoute] string name)
        {
            var image = FilesHelper.GetPhoto("wwwroot\\images\\cameras", name);
            return File(image, "image/jpeg");
            //return Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/cameras", name);
        }
    }
}