using IPCViewer.Api.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IPCViewer.Api.Data
{
    public class CityRepository : GenericRepository<City>, ICityRepository
    {
        private readonly DataContext context;

        public CityRepository(DataContext context) : base(context)

        {
            this.context = context;
        }

        public async Task<City> GetCityByIdAsync(int id)
        {
            return await this.context.City.FindAsync(id);
        }

        public async Task<int> DeleteCityAsync(City city)
        {
            var City = this.context.City.Where(c => c.Id == city.Id).FirstOrDefault();
            if (City == null)
            {
                return 0;
            }

            this.context.City.Remove(City);
            await this.context.SaveChangesAsync();
            return City.Id;
        }
    }
}
