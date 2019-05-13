using IPCViewer.Api.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace IPCViewer.Api.Data
{
    public class CameraRepository : GenericRepository<Camera>, ICameraRepository
    {
        private readonly DataContext context;

        public CameraRepository (DataContext context) : base(context)
        {
            this.context = context;
        }

        public IQueryable GetAllWithUsers ()
        {
            return this.context.Cameras.Include(c => c.User).Include(c => c.City);
        }

        public Task<Camera> GetCamera (int id)
        {
            var camera = context.Cameras
                .Where(c => c.Id == id)
                .Include(c => c.User)
                .Include(c => c.City)
                .FirstOrDefaultAsync();

            if ( camera == null )
            {
                return null;
            }

            return camera;
        }
    }
}