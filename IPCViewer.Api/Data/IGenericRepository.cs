namespace IPCViewer.Api.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    /**
     * Interfaz del repositorio generico
     *
     * De aquí heredaran las demas clases
     * todas las clases compartiran estos metodos
     */

    public interface IGenericRepository<T> where T : class
    {
        IQueryable<T> GetAll ();

        Task<T> GetByIdAsync (int id);

        Task<T> CreateAsync (T entity);

        Task<T> UpdateAsync (T entity);

        Task DeleteAsync (T entity);
    }
}