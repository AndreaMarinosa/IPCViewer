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
    public class CamerasController : ControllerBase
    {
        private readonly DataContext _context;

        public CamerasController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Cameras
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Camera>>> GetCameras()
        {
            return await _context.Cameras.ToListAsync();
        }

        // GET: api/Cameras/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Camera>> GetCamera(int id)
        {
            var camera = await _context.Cameras.FindAsync(id);

            if (camera == null)
            {
                return NotFound();
            }

            return camera;
        }

        // PUT: api/Cameras/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCamera(long id, Camera camera)
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

        // POST: api/Cameras
        [HttpPost]
        public async Task<ActionResult<Camera>> PostCamera(Camera camera)
        {
            _context.Cameras.Add(camera);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCamera", new { id = camera.Id }, camera);
        }

        // DELETE: api/Cameras/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Camera>> DeleteCamera(long id)
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

        private bool CameraExists(long id)
        {
            return _context.Cameras.Any(e => e.Id == id);
        }
    }
}
