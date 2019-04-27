using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IPCViewer.Api.Models;
using IPCViewer.Api.Helpers;

namespace IPCViewer.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CamerasController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IUserHelper userHelper;

        public CamerasController(DataContext context, IUserHelper userHelper)
        {
            _context = context;
            this.userHelper = userHelper;
        }

        // GET: api/Cameras
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Camera>>> GetCameras()
        {
            return await _context.Cameras.Include(c => c.User).Include(c => c.City).ToListAsync();
        }

        // GET: api/Cameras/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Camera>> GetCamera(int id)
        {
            var camera = await _context.Cameras
                .Where(c => c.Id == id)
                .Include(c => c.User)
                .Include(c => c.City)
                .FirstOrDefaultAsync();

            if (camera == null)
            {
                return NotFound();
            }

            return camera;
        }

        // PUT: api/Cameras/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCamera(int id, Camera camera)
        {
            if (id != camera.Id)
            {
                return BadRequest();
            }

            _context.Entry(camera).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CameraExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

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

            //var oldCamera = await this.cameraRepository.GetByIdAsync(id);
            //if (oldCamera == null)
            //{
            //    return this.BadRequest("Camera Id don't exists.");
            //}

            ////TODO: Upload images
            //oldCamera.IsAvailabe = camera.IsAvailabe;
            //oldCamera.LastPurchase = camera.LastPurchase;
            //oldCamera.LastSale = camera.LastSale;
            //oldCamera.Name = camera.Name;
            //oldCamera.Price = camera.Price;
            //oldCamera.Stock = camera.Stock;

            //var updatedCamera = await this.cameraRepository.UpdateAsync(oldCamera);
            //return Ok(updatedCamera);
            return Ok();
        }


        // POST: api/Cameras
        [HttpPost]
        public async Task<ActionResult<Camera>> PostCamera(Camera camera)
        {
            _context.Cameras.Add(camera);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCamera", new { id = camera.Id }, camera);
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
                User = user
            };

            //var newCamera = await this.CreateAsync(entityCamera);
            //return Ok(newCamera);
            return Ok();
        }


        // DELETE: api/Cameras/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Camera>> DeleteCamera(int id)
        {
            var camera = await _context.Cameras.FindAsync(id);
            if (camera == null)
            {
                return NotFound();
            }

            _context.Cameras.Remove(camera);
            await _context.SaveChangesAsync();

            return camera;
        }

        private bool CameraExists(int id)
        {
            return _context.Cameras.Any(e => e.Id == id);
        }
    }
}
