namespace IPCViewer.Api.Models
{
    /**
     * Clase city la cual tendran tanto las camaras como los usuarios
     * como manera de ordenacion
     */

    public class City : IEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}