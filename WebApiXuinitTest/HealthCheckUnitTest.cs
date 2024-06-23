using Adventure.Works._2012.dbContext.HealthCheck;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace WebApiXuinitTest
{
    public class HealthCheckUnitTest
    {
        [Fact]
        public async Task HealthCheckAdventureDatabaseHealthy()
        {
            //Arrange
            var connectionString = "Data Source=dacpc\\msqlserver;Initial Catalog=AdventureWorksDW2012;User ID=sa;Password=1qaz@WSX";
            HealthCheckContext context = new HealthCheckContext();
            CancellationToken cancellationToken = default(CancellationToken);
            var mockConfSection = new Mock<IConfigurationSection>();
            mockConfSection.SetupGet(x => x[It.Is<string>(x => x  =="AdventureWorks")]).Returns(connectionString);
            mockConfSection.Setup(x => x.GetSection(It.Is<string>(s=> s =="ConnectionStrings"))).Returns(mockConfSection.Object);

            //act
            var databaseHealthCheck = new AdventureWorksHealthCheck(mockConfSection.Object);
            context.Registration = new HealthCheckRegistration("DatabaseHealthCheck",
                databaseHealthCheck, HealthStatus.Healthy, new[] { "database" });
            var result = await databaseHealthCheck.CheckHealthAsync(context, cancellationToken);

            //assert
            Assert.Equal("Healthy", result.Status.ToString());
        }

        [Fact]
        public async Task HealthCheckAdventureDatabaseUnHealthy()
        {
            //Arrange
            var connectionString = "Data Source=dacpc\\msqlserver;Initial Catalog=AdventureWorksDW2012;User ID=fake;Password=fakepassword";
            HealthCheckContext context = new HealthCheckContext();
            CancellationToken cancellationToken = default(CancellationToken);
            var mockConfSection = new Mock<IConfigurationSection>();
            mockConfSection.SetupGet(x => x[It.Is<string>(x => x  =="AdventureWorks")]).Returns(connectionString);
            mockConfSection.Setup(x => x.GetSection(It.Is<string>(s => s =="ConnectionStrings"))).Returns(mockConfSection.Object);

            //act
            var databaseHealthCheck = new AdventureWorksHealthCheck(mockConfSection.Object);
            context.Registration = new HealthCheckRegistration("DatabaseHealthCheck",
                databaseHealthCheck, HealthStatus.Unhealthy, new[] { "database" });
            var result = await databaseHealthCheck.CheckHealthAsync(context, cancellationToken);

            //assert
            Assert.Equal("Unhealthy", result.Status.ToString());
        }
    }
}
