namespace IPCViewer.Api.Models
{
    using System;

    /**
     * Objeto con el que se almacenaran todas las caramas de la aplicacion
     */

    public class Camera : IEntity
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

        public int CityId { get; set; }

        public string ImageFullPath
        {
            get
            {
                if ( string.IsNullOrEmpty(this.ImageUrl) )
                {
                    return null;
                }
                else if ( ImageUrl.StartsWith("http") )
                {
                    return this.ImageUrl;
                }

                return $"https://ipcviewerapi.azurewebsites.net/api/cameras/image/{this.ImageUrl}";
            }
        }
    }
}