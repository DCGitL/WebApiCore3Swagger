using EmployeeDB.Dal.Employee.DbRepository;
using EmployeeDB.Dal.EmployeeDbResponseModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Update;
using Moq;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiCore3Swagger.Controllers.EmployeeDb;
using WebApiXuinitTest.MockRepositories;
using Xunit;

namespace WebApiXuinitTest
{
    public class EmployeeDbControllerTest
    {
        private readonly Mock<IEmployeeDbRepository> mockDbEmployees; 
        public EmployeeDbControllerTest()
        {
            mockDbEmployees = new Mock<IEmployeeDbRepository>();
        }

        [Fact]
        public async Task GetAllDbEmployeesStatus200Test()
        {
            //Arrange 
       
            var dbEmployees = new MockEmployeeDbRepository().GetEmployeeDbsAsync();
            mockDbEmployees.Setup(e => e.GetEmployeeDbsAsync()).Returns(dbEmployees);

            var employeeDbController = new EmployeeDbController(mockDbEmployees.Object);

            //Act
          
            var result = await employeeDbController.GetallDbEmployees();

            //Assert
            Assert.NotNull(result);
            var objectResult =   Assert.IsType<OkObjectResult>(result.Result);

            Assert.Equal(200, objectResult.StatusCode);
            var returnResults = objectResult.Value as IEnumerable<EmployeeDbResponse>;
            Assert.NotNull(returnResults);
            var count = returnResults.Count<EmployeeDbResponse>();
            Assert.Equal(3, count);

        }

        [Fact]
        public async Task GetDbEmployeeStatus200Test()
        {
            var employeeid = 1;
            //Arrange 
            var dbEmployee =  new MockEmployeeDbRepository().GetEmployeeDbAsync(employeeid);
            mockDbEmployees.Setup(e => e.GetEmployeeDbAsync(employeeid)).Returns(dbEmployee);

            var employeeDbController = new EmployeeDbController(mockDbEmployees.Object);
             

            //Act 
            var result = await employeeDbController.GetDbEmployee(employeeid);

            //Assert
            Assert.NotNull(result);
            var objectResult = Assert.IsType<OkObjectResult>(result.Result);

            Assert.Equal(200, objectResult.StatusCode);
            var returnResult = objectResult.Value as EmployeeDbResponse;
            Assert.NotNull(returnResult);
            Assert.Equal("David", returnResult.FirstName);
            Assert.Equal("Chen", returnResult.LastName);
            Assert.Equal("Male", returnResult.Gender);
        }

        [Fact]
        public async Task CreateDbEmployee()
        {
            EmployeeDbResponse emp = new EmployeeDbResponse
            {
                Id = 4,
                FirstName = "Savannah",
                LastName = "Stephen",
                Gender = "Female",
                Salary = 8500.90m
            };

            //Arrange
            var dbEmployee = new MockEmployeeDbRepository().CreateEmployeeDbAsync(emp);
            mockDbEmployees.Setup( e =>  e.CreateEmployeeDbAsync(emp)).Returns(dbEmployee);

            var employeeDbController = new EmployeeDbController(mockDbEmployees.Object);
            var expectedLink = "http://localHost/api/v3.1/EmployeeDb/GetDbEmployee/4";
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Scheme = "http";
            httpContext.Request.Host = new HostString("localHost");
            httpContext.Request.PathBase ="/api/v3.1/EmployeeDb";
            httpContext.Request.Path = "/CreateEmployee";

            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };

            employeeDbController.ControllerContext = controllerContext;
           

            //Act
            var result = await employeeDbController.CreateEmployee(emp);
         
            //Assert
            Assert.NotNull(result);
            var objectResult = Assert.IsType<CreatedResult>(result.Result);
            var location = objectResult.Location;
            var createdEmployee = objectResult.Value as EmployeeDbResponse;
            Assert.NotNull(createdEmployee);
            Assert.Equal(expectedLink, location, ignoreCase: true);
        }
    }
}
