using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestLib.Context;
using TestLib.ResponseModels;

namespace TestLib.AdventureWorks.Repository
{
    public class AdventureWorksRepository : IAdventureWorksRepository
    {
        private readonly AdventureWorksDbContext adventureWorksDbContext;

        public AdventureWorksRepository(AdventureWorksDbContext adventureWorksDbContext)
        {
            this.adventureWorksDbContext = adventureWorksDbContext;
        }
        public async Task<IEnumerable<ResponseAwEmployee>> GetAdventureWorksEmployeesAsync()
        {
            var results =  await Task.Run(() =>
            {
                return    adventureWorksDbContext.DimEmployee.Select(e => new ResponseAwEmployee
                {
                    EmployeeKey = e.EmployeeKey,
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    MiddleName = e.MiddleName,
                    EmailAddress = e.EmailAddress,
                    Phone = e.Phone,
                    StartDate = e.StartDate,
                    EndDate = e.EndDate,
                    DepartmentName = e.DepartmentName
                });
            });


            return results;  // await Task.FromResult(employees);
        }

        public async Task<ResponseAwEmployee> GetEmployee(int employeeID)
        {

            var employee = await Task.Run(() =>
            {
                return adventureWorksDbContext.DimEmployee.Select(e => new ResponseAwEmployee
                {
                    EmployeeKey = e.EmployeeKey,
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    MiddleName = e.MiddleName,
                    EmailAddress = e.EmailAddress,
                    Phone = e.Phone,
                    DepartmentName = e.DepartmentName,
                    StartDate = e.StartDate,
                    EndDate = e.EndDate
                }).FirstOrDefault(e => e.EmployeeKey == employeeID);
            } );

            return employee;
        }

        public async Task<MemoryStream> GetEmployeePhoto(int employeeID)
        {
            var employee =  adventureWorksDbContext.DimEmployee.FirstOrDefault(e => e.EmployeeKey == employeeID);
            var photo = employee?.EmployeePhoto;
            MemoryStream myphoto = await Task.FromResult( new MemoryStream(photo,0,photo.Length));
            
           

            return myphoto;
        }
    }
}
