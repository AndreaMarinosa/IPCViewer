using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IPCViewer.Api.Models
{
    public class Camera
    {

        public int Id { get; set; }

        public string Name { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public string Comments { get; set; }

        public string ImageUrl { get; set; }

        public DateTime CreatedDate { get; set; }

        public User User { get; set; }

        public City City { get; set; }

        public string ImageFullPath
        {
            get
            {
                if (string.IsNullOrEmpty(this.ImageUrl))
                {
                    return null;
                }

                return $"https://ipcviewer.azurewebsites.net{ImageUrl.Substring(1)}";
            }
        }

    }
}
