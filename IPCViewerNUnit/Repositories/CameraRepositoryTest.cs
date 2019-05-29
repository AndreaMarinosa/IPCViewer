using System.Linq;
using System.Threading.Tasks;
using IPCViewer.Api.Data;
using IPCViewer.Api.Models;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace IPCViewerNUnit.Repositories
{
    [TestFixture]
    public class CameraRepositoryTest
    {
        private readonly DbContextOptions<DataContext> options;

        public CameraRepositoryTest ()
        {
            var builder = new DbContextOptionsBuilder<DataContext>();
            builder.UseInMemoryDatabase("test_database");
            options = builder.Options;
        }

        [Test]
        public void GetAllWithUsersReturnMoreThanZero ()
        {
            var getAll = 0;

            // Act (Do test)
            using ( var context = new DataContext(options) )
            {
                var repository = new CameraRepository(context);
                getAll = repository.GetAllWithUsers().Cast<IQueryable>().Count();
            }

            // Assert (Check results).
            using ( var context = new DataContext(options) )
            {
                Assert.AreEqual(getAll, Is.GreaterThan(1), "No llegan usuarios.");
            }

        }

        [Test]
        public void FindCamerasThroughNameIsWorking ()
        {

            // Act (Do test)
            using ( var context = new DataContext(options: options) )
            {
                context.Cameras.Add(entity: new Camera
                {
                    Name = "TestCamera2",
                    User = context.Users.FirstOrDefault(),
                    City = context.City.FirstOrDefault()
                });

                context.Cameras.Add(entity: new Camera
                {
                    Name = "AnotherTestCamera",
                    User = context.Users.FirstOrDefault(),
                    City = context.City.FirstOrDefault()
                });

                context.SaveChanges();
            }

            // Assert (Check results).
            using ( var context = new DataContext(options: options) )
            {
                var repository = new CameraRepository(context: context);
                var result = repository.GetCamera(context.Cameras.FirstOrDefault(c => c.Name == "TestCamera").Id);

                Assert.AreEqual(expected: result.Result.Name, actual: Is.EqualTo(expected: "TestCamera2"), message: "No se encuentra el usuario.");
            }
        }
    }
}
