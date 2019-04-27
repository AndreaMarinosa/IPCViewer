using IPCViewer.Api.Models;
using System.Linq;

namespace IPCViewer.Api.Data
{
    public interface ICameraRepository : IGenericRepository<Camera>
    {
        IQueryable GetAllWithUsers();
    }
}