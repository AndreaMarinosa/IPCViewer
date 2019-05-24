using IPCViewer.Api.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IPCViewer.Api.Data
{
    public class DataContext : IdentityDbContext<User>
    {
        public DbSet<Camera> Cameras { get; set; }

        public DbSet<City> City { get; set; }

        public DataContext (DbContextOptions<DataContext> options) : base(options)
        {
        }

        protected override void OnModelCreating (ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Camera>();

            //var cascadeFKs = modelBuilder.Model
            //    .G­etEntityTypes()
            //    .SelectMany(t => t.GetForeignKeys())
            //    .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Casca­de);
            //foreach ( var fk in cascadeFKs )
            //{
            //    fk.DeleteBehavior = DeleteBehavior.Restr­ict;
            //}

            base.OnModelCreating(modelBuilder);
        }
    }
}