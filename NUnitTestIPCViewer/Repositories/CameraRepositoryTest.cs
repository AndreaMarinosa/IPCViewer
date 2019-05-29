using IPCViewer.Api.Data;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace NUnitTestIPCViewer.Repositories
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
            // Arrange (Initialization)

            // Act (Do test)
            using ( var context = new DataContext(options) )
            {
                var repository = new CameraRepository(context);

            }
            // Assert (Check results)
            // TODO: Add your test code here
            var answer = 42;
            Assert.That(answer, Is.EqualTo(42), "Some useful error message");
        }
    }
}
