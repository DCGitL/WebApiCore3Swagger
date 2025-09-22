using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TestLib.Context;
using TestLib.ResponseModels;

namespace TestLib.AdventureWorks.Repository
{
    public class AdventureWorksRepository(AdventureWorksDbContext adventureWorksDbContext) : IAdventureWorksRepository
    {
    

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

           
            
                return await adventureWorksDbContext.DimEmployee.Select(e => new ResponseAwEmployee
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
                }).FirstOrDefaultAsync(e => e.EmployeeKey == employeeID);
          

          
        }

        public async Task<MemoryStream> GetEmployeePhoto(int employeeID)
        {
            var employee = await  adventureWorksDbContext.DimEmployee.FirstOrDefaultAsync(e => e.EmployeeKey == employeeID);
            var photo = employee?.EmployeePhoto;
            MemoryStream myphoto = new MemoryStream(photo,0,photo.Length);
            
           

            return myphoto;
        }
    }
}
