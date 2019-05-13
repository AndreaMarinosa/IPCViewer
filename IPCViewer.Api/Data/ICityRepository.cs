namespace IPCViewer.Api.Data
{
    using IPCViewer.Api.Models;
    using System.Threading.Tasks;

    /**
     * A parte de los metodos genericos,
     * cuenta con los propios
     */

    public interface ICityRepository : IGenericRepository<City>
    {
        Task<City> GetCityByIdAsync (int id);

        Task<int> DeleteCityAsync (City city);
    }
}