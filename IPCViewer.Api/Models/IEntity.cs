namespace IPCViewer.Api.Models
{
    /**
     * Clase generica de la que heredan las demas clases
     * donde comparten el mismo Id 
     */
    public interface IEntity
    {
        int Id { get; set; }
    }

}
