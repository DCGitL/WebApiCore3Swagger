using AutoMapper;
using EmployeeDB.Dal.EmployeeDbResponseModels;
using EmployeeDB.Dal.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EmployeeDB.Dal.Employee.DbRepository
{
    public class EmployeeDbRepository : IEmployeeDbRepository
    {
        private readonly EmployeeDbContext employeeDbContext;
        private readonly IMapper mapper;

        public EmployeeDbRepository(EmployeeDbContext employeeDbContext, IMapper mapper)
        {
            this.employeeDbContext = employeeDbContext;
            this.mapper = mapper;
        }
        public async Task<EmployeeDbResponse> CreateEmployeeDbAsync(EmployeeDbResponse DbEmployee,CancellationToken cancellationToken)
        {
            var employee = mapper.Map<Employees>(DbEmployee);

            await employeeDbContext.Employees.AddAsync(employee,cancellationToken);
            await employeeDbContext.SaveChangesAsync();

            var returnval = mapper.Map<EmployeeDbResponse>(employee);

            return returnval;

        }

        public async Task<EmployeeDbResponse> UpdateEmployeDbAsync(EmployeeDbResponse DbEmployee, CancellationToken cancellationToken)
        {
            var existEmployee = await employeeDbContext.Employees.FirstOrDefaultAsync(e => e.Id == DbEmployee.Id,cancellationToken);
            if (existEmployee != null)
            {
                existEmployee.FirstName = DbEmployee.FirstName;
                existEmployee.LastName = DbEmployee.LastName;
                existEmployee.Gender = DbEmployee.Gender;
                existEmployee.Salary = DbEmployee.Salary;
               await  employeeDbContext.SaveChangesAsync(cancellationToken);
            }

            return  mapper.Map<EmployeeDbResponse>(existEmployee);

        }
        public async Task DeleteEmployeeDbAsync(int id, CancellationToken cancellation)
        {

            var employee = await employeeDbContext.Employees.FirstOrDefaultAsync(e => e.Id == id,cancellation);
            if (employee != null)
            {
                employeeDbContext.Employees.Remove(employee);
                await employeeDbContext.SaveChangesAsync(cancellation);
            }
        }

        public async Task<EmployeeDbResponse> GetEmployeeDbAsync(int id, CancellationToken cancellation)
        {
            var result = await employeeDbContext.Employees.FindAsync(id,cancellation);
            var returnval = mapper.Map<EmployeeDbResponse>(result);

            return returnval;
        }

        public async Task<IEnumerable<EmployeeDbResponse>> GetEmployeeDbsAsync(CancellationToken cancellation)
        {
            var resultlist = await employeeDbContext.Employees.ToListAsync(cancellation);

            var returvals = mapper.Map<List<EmployeeDbResponse>>(resultlist);

            return returvals;
        }
    }
}
