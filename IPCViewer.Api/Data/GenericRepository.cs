namespace IPCViewer.Api.Data
{
    using IPCViewer.Api.Models;
    using Microsoft.EntityFrameworkCore;
    using System.Linq;
    using System.Threading.Tasks;

    /**
     * Heredamos la interfaz con '<T>', es decir, cualquier clase
     * Dont '<T>' hereda de la clase Entity, donde la propiedad que comparten todas las
     * clases es el Id
     */
    public class GenericRepository<T> : IGenericRepository<T> where T : class, IEntity
    {
        // Le pasamos el datacontext
        private readonly DataContext context;

        public GenericRepository(DataContext context)
        {
            this.context = context;
        }

        /**
         * Coge todos los objetos de la clase <T>
         */
        public IQueryable<T> GetAll()
        {
            return this.context.Set<T>().AsNoTracking();
        }

        /**
         * Busca y devuelve el el objeto con el id pasado por parametro
         */
        public async Task<T> GetByIdAsync(int id)
        {
            return await this.context.Set<T>()
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id);
        }


        /**
         * Creamos una nueva entidad, la cual
         * ha sido pasada por parametro,
         * guardamos cambios en bbdd
         * y devolvemos la entidad
         */
        public async Task<T> CreateAsync(T entity)
        {
            await this.context.Set<T>().AddAsync(entity);
            await SaveAllAsync();
            return entity;
        }

        /**
         * Actualizamos una entidad la cual ha sido
         * pasada por parametro,
         * guardamos los cambios en la bbdd
         * Devolvemos el objeto actualizado
         *
         */
        public async Task<T> UpdateAsync(T entity)
        {
            this.context.Set<T>().Update(entity);
            await SaveAllAsync();
            return entity;
        }

        /**
         * Eliminamos el objeto 
         */
        public async Task DeleteAsync(T entity)
        {
            this.context.Set<T>().Remove(entity);
            await SaveAllAsync();
        }

        /**
         * Metodo que guarda los cambios en la base de datos
         */
        public async Task<bool> SaveAllAsync()
        {
            return await this.context.SaveChangesAsync() > 0;
        }
    }

}
