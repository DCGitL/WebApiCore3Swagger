using EmployeeDB.Dal.Employee.DbRepository;
using EmployeeDB.Dal.EmployeeDbResponseModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WebApiXuinitTest.MockRepositories
{
    public class MockEmployeeDbRepository : IEmployeeDbRepository
    {
        private readonly IList<EmployeeDbResponse> dbemployees;
        public MockEmployeeDbRepository()
        {
            dbemployees = new List<EmployeeDbResponse>() { 
                new EmployeeDbResponse {FirstName="David", LastName="Chen", Gender="Male", Id=1, Salary = 90000.06m },
                  new EmployeeDbResponse {FirstName="Shannon", LastName="Miller", Gender="Female", Id=2, Salary = 95000.06m },
                   new EmployeeDbResponse {FirstName="Zeena", LastName="Tutunji", Gender="Female", Id=3, Salary = 95000.06m },
                };
        }
        public async Task<EmployeeDbResponse> CreateEmployeeDbAsync(EmployeeDbResponse DbEmployee, CancellationToken cancellation)
        {

           if (cancellation.IsCancellationRequested)
            {
               await Task.FromCanceled(cancellation);
            }
            dbemployees.Add(DbEmployee);
            return await Task.FromResult(DbEmployee);
            
        }

        public async Task DeleteEmployeeDbAsync(int id, CancellationToken cancellation)
        {


            var employee = dbemployees.FirstOrDefault(e => e.Id == id);
            if (cancellation.IsCancellationRequested)
            {
                await Task.FromCanceled(cancellation);
            }
            if (employee != null)
            {
                dbemployees.Remove(employee);
            }

            await Task.FromResult(0);
        }
        
        public async Task<EmployeeDbResponse> GetEmployeeDbAsync(int id, CancellationToken cancellation)
        {
            if (cancellation.IsCancellationRequested)
            {
                await Task.FromCanceled(cancellation);
            }
            var employee = dbemployees.FirstOrDefault(e => e.Id == id);
            return await Task.FromResult(employee);
        }

        public async  Task<IEnumerable<EmployeeDbResponse>> GetEmployeeDbsAsync(CancellationToken cancellation)
        {
            return await Task.FromResult(dbemployees.AsEnumerable<EmployeeDbResponse>());
        }

        public async Task<EmployeeDbResponse> UpdateEmployeDbAsync(EmployeeDbResponse DbEmployee, CancellationToken cancellation)
        {
            var existEmployee = dbemployees.FirstOrDefault(e => e.Id == DbEmployee.Id);
            if (existEmployee != null)
            {
                existEmployee.FirstName = DbEmployee.FirstName;
                existEmployee.LastName = DbEmployee.LastName;
                existEmployee.Gender = DbEmployee.Gender;
                existEmployee.Salary = DbEmployee.Salary;
                
            }

            return await Task.FromResult(existEmployee);
        }
    }
}
