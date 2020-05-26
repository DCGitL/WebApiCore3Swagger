using AutoMapper;
using EmployeeDB.Dal.EmployeeDbResponseModels;
using EmployeeDB.Dal.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
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
        public async Task<EmployeeDbResponse> CreateEmployeeDbAsync(EmployeeDbResponse DbEmployee)
        {
            var employee = mapper.Map<Employees>(DbEmployee);

            await employeeDbContext.Employees.AddAsync(employee);
            await employeeDbContext.SaveChangesAsync();

            var returnval = mapper.Map<EmployeeDbResponse>(employee);

            return returnval;

        }

        public async Task<EmployeeDbResponse> UpdateEmployeDbAsync(EmployeeDbResponse DbEmployee)
        {
            var existEmployee = await employeeDbContext.Employees.FirstOrDefaultAsync(e => e.Id == DbEmployee.Id);
            if (existEmployee != null)
            {
                existEmployee.FirstName = DbEmployee.FirstName;
                existEmployee.LastName = DbEmployee.LastName;
                existEmployee.Gender = DbEmployee.Gender;
                existEmployee.Salary = DbEmployee.Salary;
               await  employeeDbContext.SaveChangesAsync();
            }

            return  mapper.Map<EmployeeDbResponse>(existEmployee);

        }
        public async Task DeleteEmployeeDbAsync(int id)
        {

            var employee = await employeeDbContext.Employees.FirstOrDefaultAsync(e => e.Id == id);
            if (employee != null)
            {
                employeeDbContext.Employees.Remove(employee);
                await employeeDbContext.SaveChangesAsync();
            }
        }

        public async Task<EmployeeDbResponse> GetEmployeeDbAsync(int id)
        {
            var result = await employeeDbContext.Employees.FindAsync(id);
            var returnval = mapper.Map<EmployeeDbResponse>(result);

            return returnval;
        }

        public async Task<IEnumerable<EmployeeDbResponse>> GetEmployeeDbsAsync()
        {
            var resultlist = await employeeDbContext.Employees.ToListAsync();

            var returvals = mapper.Map<List<EmployeeDbResponse>>(resultlist);

            return returvals;
        }
    }
}
