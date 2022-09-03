using EmployeeDB.Dal.Employee.DbRepository;
using EmployeeDB.Dal.EmployeeDbResponseModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebApiCore3Swagger.Controllers.EmployeeDb;
using WebApiCore3Swagger.NLogger;
using WebApiXuinitTest.MockRepositories;
using Xunit;

namespace WebApiXuinitTest
{
    public class EmployeeDbControllerTest
    {
        private readonly Mock<IEmployeeDbRepository> mockDbEmployees;
        private readonly ILogger<EmployeeDbController> loggerMock;
        private readonly ICustomNlogProperties factorycustLog;
        private readonly CancellationToken _cancellationToken;
        public EmployeeDbControllerTest()
        {
            mockDbEmployees = new Mock<IEmployeeDbRepository>();
            loggerMock = Mock.Of<ILogger<EmployeeDbController>>();
            var serviceProvider = new ServiceCollection()
               .AddSingleton<ICustomNlogProperties, CustomNlogProperties>()
               .BuildServiceProvider();
            factorycustLog = serviceProvider.GetService<ICustomNlogProperties>();
            _cancellationToken = new CancellationToken();
        }

        [Fact]
        public async Task GetAllDbEmployeesStatus200Test()
        {
            //Arrange 
           
            var mockRepository = new MockEmployeeDbRepository();
            var dbEmployees = await mockRepository.GetEmployeeDbsAsync(_cancellationToken);
             mockDbEmployees.Setup(e => e.GetEmployeeDbsAsync(_cancellationToken)).ReturnsAsync(dbEmployees);
           
            var employeeDbController = new EmployeeDbController(mockDbEmployees.Object, loggerMock, factorycustLog);
            

            //Act
          
            var result = await employeeDbController.GetallDbEmployees(_cancellationToken);

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
            var dbEmployee = await  new MockEmployeeDbRepository().GetEmployeeDbAsync(employeeid, _cancellationToken);
            mockDbEmployees.Setup(e => e.GetEmployeeDbAsync(employeeid, _cancellationToken)).ReturnsAsync(dbEmployee);

            var employeeDbController = new EmployeeDbController(mockDbEmployees.Object, loggerMock, factorycustLog);
             

            //Act 
            var result = await employeeDbController.GetDbEmployee(employeeid, _cancellationToken);

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
            //Arrange
            EmployeeDbResponse emp = new EmployeeDbResponse
            {
                Id = 4,
                FirstName = "Savannah",
                LastName = "Stephen",
                Gender = "Female",
                Salary = 8500.90m
            };

            
            var dbEmployee = await new MockEmployeeDbRepository().CreateEmployeeDbAsync(emp, _cancellationToken);
            mockDbEmployees.Setup( e =>  e.CreateEmployeeDbAsync(emp, _cancellationToken)).ReturnsAsync(dbEmployee);

            var employeeDbController = new EmployeeDbController(mockDbEmployees.Object, loggerMock, factorycustLog);
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
            var result = await employeeDbController.CreateEmployee(emp, _cancellationToken);
         
            //Assert
            Assert.NotNull(result);
            var objectResult = Assert.IsType<CreatedResult>(result.Result);
            var location = objectResult.Location;
            var createdEmployee = objectResult.Value as EmployeeDbResponse;
            Assert.NotNull(createdEmployee);
            Assert.Equal(expectedLink, location, ignoreCase: true);
        }


        [Fact]
        public async Task DeletDbEmployee()
        {
            var employeeid = 1;
            //Arrange
            var mockRepository = new MockEmployeeDbRepository();
            var returnval =   mockRepository.DeleteEmployeeDbAsync(employeeid, _cancellationToken);
            mockDbEmployees.Setup(e => e.DeleteEmployeeDbAsync(employeeid, _cancellationToken)).Returns(returnval);

            var employeeDbController = new EmployeeDbController(mockDbEmployees.Object, loggerMock, factorycustLog);
            //Act

            var result = await employeeDbController.DeleteDbEmployee(employeeid, _cancellationToken);

            //Assert
            Assert.NotNull(result);
            var objectResult = Assert.IsType<OkObjectResult>(result.Result);
            var statuscode = objectResult.StatusCode;
            Assert.Equal(200, statuscode);
            var val =  objectResult.Value == null? string.Empty : objectResult.Value.ToString();
            Assert.Equal("Record successfully deleted", val, ignoreCase: true);
        }


        [Fact]
        public async Task UpdateDbEmployee()
        {
            //Arrange
            EmployeeDbResponse emp = new EmployeeDbResponse
            {
                Id = 1,
                FirstName = "David",
                LastName = "Chen",
                Gender = "Male",
                Salary = 100000.90m
            };
            var mockRepository = new MockEmployeeDbRepository();
            var returnval = mockRepository.UpdateEmployeDbAsync(emp, _cancellationToken);
            mockDbEmployees.Setup(e => e.UpdateEmployeDbAsync(emp, _cancellationToken)).Returns(returnval);

            var employeeDbController = new EmployeeDbController(mockDbEmployees.Object, loggerMock, factorycustLog);

            //Act

            var result = await employeeDbController.UpdateDbEmployee(emp, _cancellationToken);


            //Assert
            Assert.NotNull(result);
            var objectResult = Assert.IsType<OkObjectResult>(result.Result);
            var statuscode = objectResult.StatusCode;
            Assert.Equal(200, statuscode);
            var returnResult = objectResult.Value as EmployeeDbResponse;
            Assert.NotNull(returnResult);
            Assert.Equal("David", returnResult.FirstName);
            Assert.Equal("Chen", returnResult.LastName);
            Assert.Equal("Male", returnResult.Gender);
            Assert.Equal(100000.90m, returnResult.Salary);
        }

        
        [Fact]
        public async Task UpdateDbEmployee_NotFound()
        {
            //Arrange
            EmployeeDbResponse emp = new EmployeeDbResponse
            {
                Id = 6,
                FirstName = "David",
                LastName = "Chen",
                Gender = "Male",
                Salary = 100000.90m
            };
            var mockRepository = new MockEmployeeDbRepository();
            var returnval = mockRepository.UpdateEmployeDbAsync(emp, _cancellationToken);
            mockDbEmployees.Setup(e => e.UpdateEmployeDbAsync(emp, _cancellationToken)).Returns(returnval);

            var employeeDbController = new EmployeeDbController(mockDbEmployees.Object, loggerMock, factorycustLog);

            //Act
            var result = await employeeDbController.UpdateDbEmployee(emp, _cancellationToken);


            //Assert
            Assert.NotNull(result);
            var objectNotResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            var statuscode = objectNotResult.StatusCode;
            Assert.Equal(404, statuscode);
        }
    }
}
