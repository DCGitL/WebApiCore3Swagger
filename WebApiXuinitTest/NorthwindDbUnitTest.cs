using Adventure.Works._2012.dbContext.Northwind.Repository;
using Adventure.Works._2012.dbContext.ResponseModels;
using System.Linq;
using System.Threading.Tasks;
using WebApiXuinitTest.Helper;
using Xunit;

namespace WebApiXuinitTest
{
    public class NorthwindDbUnitTest : IClassFixture<NorthwindDbTestFixture>
    {
        private readonly NorthwindDbTestFixture _fixture;

        public NorthwindDbUnitTest(NorthwindDbTestFixture fixture)
        {
            _fixture=fixture;
        }

        [Fact]
        public async Task GetAllEmployeesShouldHaveThreeEmploees()
        {
            var repo = new NorthwindRepository(_fixture.DbContext);

            var result = await repo.GetAllAsyncEmployees();
            Assert.NotNull(result);
            Assert.Equal(3, result.Count<ResponseEmployee>());

        }
    }
}