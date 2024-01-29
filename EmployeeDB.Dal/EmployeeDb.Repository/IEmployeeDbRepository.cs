using EmployeeDB.Dal.EmployeeDbResponseModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EmployeeDB.Dal.Employee.DbRepository
{
    public interface IEmployeeDbRepository
    {

        public Task<IEnumerable<EmployeeDbResponse>> GetEmployeeDbsAsync(CancellationToken cancellation);

        public Task<EmployeeDbResponse> GetEmployeeDbAsync(int id, CancellationToken cancellation);

        public Task<EmployeeDbResponse> CreateEmployeeDbAsync(EmployeeDbResponse DbEmployee, CancellationToken cancellation);

        public Task<EmployeeDbResponse> UpdateEmployeDbAsync(EmployeeDbResponse DbEmployee, CancellationToken cancellation);

        public Task DeleteEmployeeDbAsync(int id, CancellationToken cancellation);
    }
}
