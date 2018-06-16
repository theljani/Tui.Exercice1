using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TUI.Flights.Infrastructure.Tests
{
    [TestClass]
    public class DbContextTests
    {
        DbContextOptionsBuilder<EFUnitOfWork> _contextBuilder;

        [TestInitialize]
        public void Init()
        {
            // Use inMemory database
            _contextBuilder = new DbContextOptionsBuilder<EFUnitOfWork>();
            _contextBuilder.UseInMemoryDatabase(databaseName: "TestDb");
        }


        [TestMethod]
        public void UnitOfWorkTests_ShouldInitializeUnitOfWork()
        {

            // Arrange
            var builder = _contextBuilder.Options;
            var dbContext = new EFUnitOfWork(builder);

            // Assert
            Assert.IsNotNull(dbContext);
        }
    }
}
