using IPCViewer.Api.Models;
using System.Linq;
using System.Threading.Tasks;

namespace IPCViewer.Api.Data
{
    /**
     * Interfaz de camara repository
     * Tiene dos metodos ademas de los genericos
     */

    public interface ICameraRepository : IGenericRepository<Camera>
    {
        // Devuelve todas las camaras mas sus usuarios
        IQueryable GetAllWithUsers ();

        Task<Camera> GetCamera (int id);
    }
}