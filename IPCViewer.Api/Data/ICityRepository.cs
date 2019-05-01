using System.Linq;
using System.Threading.Tasks;
using IPCViewer.Api.Data;

namespace IPCViewer.Api.Models
{
    public interface ICityRepository : IGenericRepository<City>
    {
        Task<City> GetCityByIdAsync(int id);

        Task<int> DeleteCityAsync(City city);
    }
}
