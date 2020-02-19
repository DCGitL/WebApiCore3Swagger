using System;
using System.Collections.Generic;
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
    }
}
