﻿namespace IPCViewer.Api.Controllers
{
    using IPCViewer.Api.Data;
    using IPCViewer.Api.Helpers;
    using IPCViewer.Api.Models;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Threading.Tasks;

    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class CamerasController : Controller
    {
        private readonly ICameraRepository cameraRepository;
        private readonly ICityRepository cityRespository;
        private readonly IUserHelper userHelper;

        public CamerasController(
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
        public IActionResult GetCameras()
        {
            return Ok(cameraRepository.GetAllWithUsers());
        }

        // GET: api/Cameras/5
        [HttpGet("{id}")]
        public IActionResult GetCamera(int id)
        {
            return Ok(cameraRepository.GetCamera(id));
        }

        //PUT 
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCamera([FromRoute] int id, [FromBody] Common.Models.Camera camera)
        {
            if (!ModelState.IsValid)
            {
                return this.BadRequest(ModelState);
            }

            if (id != camera.Id)
            {
                return BadRequest();
            }

            var oldCamera = await this.cameraRepository.GetByIdAsync(id);
            if (oldCamera == null)
            {
                return this.BadRequest("Camera Id don't exists.");
            }

            //TODO: Upload images
            oldCamera.Comments = camera.Comments;
            oldCamera.Name = camera.Name;
            oldCamera.Latitude = camera.Latitude;
            oldCamera.Longitude = camera.Longitude;
            oldCamera.CreatedDate = DateTime.Now;

            var updatedCamera = await this.cameraRepository.UpdateAsync(oldCamera);
            return Ok(updatedCamera);
        }

        // POST Camera
        [HttpPost]
        public async Task<IActionResult> PostCamera([FromBody] Common.Models.Camera camera)
        {
            if (!ModelState.IsValid)
            {
                return this.BadRequest(ModelState);
            }

            var user = await this.userHelper.GetUserByEmailAsync(camera.User.Email);
            if (user == null)
            {
                return this.BadRequest("Invalid user");
            }

            var city = await this.cityRespository.GetCityByIdAsync(camera.CityId);
            if (city == null)
            {
                return this.BadRequest("Invalid city");
            }
            // Image

            //var imageUrl = string.Empty;
            //if (camera.ImageArray != null && camera.ImageArray.Length > 0)
            //{
            //    var stream = new MemoryStream(camera.ImageArray);
            //    var guid = Guid.NewGuid().ToString();
            //    var file = $"{guid}.jpg";
            //    var folder = "wwwroot\\images\\Cameras";
            //    var fullPath = $"~/images/Cameras/{file}";
            //    var response = FilesHelper.UploadPhoto(stream, folder, file);

            //    if (response)
            //    {
            //        imageUrl = fullPath;
            //    }
            //}

            var entityCamera = new Camera
            {
                Name = camera.Name,
                Comments = camera.Comments,
                CreatedDate = DateTime.Now,
                Latitude = camera.Latitude,
                Longitude = camera.Longitude,
                User = user,
                ImageUrl = camera.ImageUrl,
                CityId = camera.CityId,
                City = city
            };

            var newCamera = await cameraRepository.CreateAsync(entityCamera);
            return Ok(newCamera);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return this.BadRequest(ModelState);
            }

            var product = await this.cameraRepository.GetByIdAsync(id);
            if (product == null)
            {
                return this.NotFound();
            }

            await this.cameraRepository.DeleteAsync(product);
            return Ok(product);
        }
    }
}
