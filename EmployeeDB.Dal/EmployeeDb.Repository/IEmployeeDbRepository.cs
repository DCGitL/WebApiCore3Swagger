using EmployeeDB.Dal.EmployeeDbResponseModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeDB.Dal.Employee.DbRepository
{
    public interface IEmployeeDbRepository
    {

        public Task<IEnumerable<EmployeeDbResponse>> GetEmployeeDbsAsync();

        public Task<EmployeeDbResponse> GetEmployeeDbAsync(int id);

        public Task<EmployeeDbResponse> CreateEmployeeDbAsync(EmployeeDbResponse DbEmployee);

        public Task<EmployeeDbResponse> UpdateEmployeDbAsync(EmployeeDbResponse DbEmployee);

        public Task DeleteEmployeeDbAsync(int id);
    }
}
