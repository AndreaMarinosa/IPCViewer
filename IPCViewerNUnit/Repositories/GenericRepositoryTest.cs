using System;
using System.Linq;
using System.Threading.Tasks;
using IPCViewer.Api.Data;
using IPCViewer.Api.Models;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace IPCViewerNUnit.Repositories
{
    [TestFixture]
    public class GenericRepositoryTest
    {
        private readonly DbContextOptions<DataContext> options;

        public GenericRepositoryTest ()
        {
            var builder = new DbContextOptionsBuilder<DataContext>();
            builder.UseInMemoryDatabase("test_database");
            options = builder.Options;
        }

        [Test]
        public void CreateAsyncCamerasReturnMoreThanZero ()
        {

            // Act (Do test)
            using ( var context = new DataContext(options) )
            {
                var repository = new GenericRepository<Camera>(context);

                var camera1 = new Camera
                {
                    Name = "TestCamera1",
                    User = context.Users.FirstOrDefault(),
                    City = context.City.FirstOrDefault()
                };


                var camera2 = new Camera
                {
                    Name = "TestCamera2",
                    User = context.Users.FirstOrDefault(),
                    City = context.City.FirstOrDefault()
                };

                repository.CreateAsync(camera1);
                repository.CreateAsync(camera2);

            }

            // Assert (Check results).
            using ( var context = new DataContext(options) )
            {
                Assert.GreaterOrEqual(context.Cameras.Count(), 2);
            }

        }

        [Test]
        public void FindCamerasByIdReturnNotNull()
        {

            // Act (Do test)
            using ( var context = new DataContext(options) )
            {
                var repository = new GenericRepository<Camera>(context);

                var camera1 = new Camera
                {
                    Name = "TestCamera1",
                    User = context.Users.FirstOrDefault(),
                    City = context.City.FirstOrDefault()
                };

                repository.CreateAsync(camera1);
            }

            // Assert (Check results).
            using ( var context = new DataContext(options: options) )
            {
                var repository = new GenericRepository<Camera>(context: context);
                var result = repository.GetByIdAsync(1);

                Assert.AreEqual(expected: result.Result.Name, "TestCamera1", message: "No se encuentra el usuario.");
            }
        }

        [Test]
        public void TestMethodInsertCamera ()
        {

            // Act (Do test)
            using ( var context = new DataContext(options) )
            {
                var repository = new GenericRepository<Camera>(context);

                var camera1 = new Camera { Name = "TestCamera1" };
                repository.CreateAsync(camera1);

                var insertedCamera = repository.GetByIdAsync(camera1.Id).Result;

                Assert.AreEqual(insertedCamera.Name, camera1.Name);
            }

        }

        [Test]
        public void TestMethodDeleteCamera ()
        {

            // Act (Do test)
            using ( var context = new DataContext(options) )
            {
                var repository = new GenericRepository<Camera>(context);

                var camera1 = new Camera { Name = "TestCamera1" };
                repository.DeleteAsync(camera1);

                var nullCamera = repository.GetByIdAsync(camera1.Id).Result;

                Assert.IsNull(nullCamera);
            }

        }
    }
}
