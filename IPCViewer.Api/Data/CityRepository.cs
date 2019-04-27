using IPCViewer.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IPCViewer.Api.Data
{
    public class CityRepository : GenericRepository<City>, ICityRepository
    {

        public CityRepository(DataContext context) : base(context)

        {

        }
    }
}
