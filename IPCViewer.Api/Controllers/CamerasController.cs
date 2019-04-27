using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IPCViewer.Api.Models;
using IPCViewer.Api.Helpers;
using IPCViewer.Api.Data;

namespace IPCViewer.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CamerasController : ControllerBase
    {
        private readonly ICameraRepository cameraRepository;
        private readonly IUserHelper userHelper;

        public CamerasController(ICameraRepository cameraRepository, IUserHelper userHelper)
        {
            this.cameraRepository = cameraRepository;
            this.userHelper = userHelper;
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
                ImageUrl = null,
                City = null
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
