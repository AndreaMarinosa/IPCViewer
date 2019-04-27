using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IPCViewer.Api.Models
{
    public class Country 
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<City> Cities { get; set; }

        public int NumberCities { get { return this.Cities == null ? 0 : this.Cities.Count; } }

    }
}
