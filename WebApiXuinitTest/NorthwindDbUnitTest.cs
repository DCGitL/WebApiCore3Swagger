using Adventure.Works._2012.dbContext.Northwind.Repository;
using Adventure.Works._2012.dbContext.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiXuinitTest.Helper;
using Xunit;

namespace WebApiXuinitTest
{
    public class NorthwindDbUnitTest : IClassFixture<NorthwinInMemoryDbContextFixture>
    {
        private readonly NorthwinInMemoryDbContextFixture _fixture;
        private readonly NorthwindRepository _repository;
        public NorthwindDbUnitTest(NorthwinInMemoryDbContextFixture fixture) 
        {
            _fixture=fixture;
            _repository = new NorthwindRepository(_fixture.Context);
        }


        [Fact]
        public async Task GetAllEmployesShouldGetAllEmployees()
        {
            var employees = await _repository.GetAllAsyncEmployees();

            var numberofEmployee = employees.Count<ResponseEmployee>();

            Assert.Equal(2, numberofEmployee);
        }

        [Fact]
        public async Task GetAllEmployesShouldGetAllEmployeesInJsonFormat()
        {
            var employees = await _repository.GetAllJsonStringEmployeesAsync();

            var result = await repo.GetAllAsyncEmployees();
            Assert.NotNull(result);
            Assert.Equal(3, result.Count<ResponseEmployee>());

            Assert.NotNull(employees);
        }
    }
}