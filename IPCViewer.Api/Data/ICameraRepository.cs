using IPCViewer.Api.Models;
using System.Linq;
using System.Threading.Tasks;

namespace IPCViewer.Api.Data
{
    public interface ICameraRepository : IGenericRepository<Camera>
    {
        IQueryable GetAllWithUsers();
        Task<Camera> GetCamera(int id);

    }
}