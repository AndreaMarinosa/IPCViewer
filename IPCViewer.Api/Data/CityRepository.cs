namespace IPCViewer.Api.Data
{
    using IPCViewer.Api.Models;
    using System.Linq;
    using System.Threading.Tasks;

    /**
     * Clase cuya funcion es definir los metodos de la interfaz ICityRepository
     */

    public class CityRepository : GenericRepository<City>, ICityRepository
    {
        private readonly DataContext context;

        public CityRepository (DataContext context) : base(context)

        {
            this.context = context;
        }

        /**
         * Devuelve la ciudad pasada por parametro
         */

        public async Task<City> GetCityByIdAsync (int id)
        {
            return await this.context.City.FindAsync(id);
        }

        /**
         * Elimina la ciudad pasada por parametro
         */

        public async Task<int> DeleteCityAsync (City City)
        {
            var city = this.context.City.FirstOrDefault(c => c.Id == City.Id);
            if ( city == null )
            {
                return 0;
            }

            this.context.City.Remove(city);
            await this.context.SaveChangesAsync();
            return city.Id;
        }
    }
}