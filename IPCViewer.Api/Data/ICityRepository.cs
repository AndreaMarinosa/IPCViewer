namespace IPCViewer.Api.Data
{
    using System.Threading.Tasks;
    using IPCViewer.Api.Models;

    /**
     * A parte de los metodos genericos,
     * cuenta con los propios
     */
    public interface ICityRepository : IGenericRepository<City>
    {
        Task<City> GetCityByIdAsync(int id);

        Task<int> DeleteCityAsync(City city);
    }
}
