using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using TestLib.ResponseModels;

namespace TestLib.AdventureWorks.Repository
{
    public interface IAdventureWorksRepository
    {
       Task<IEnumerable<ResponseAwEmployee>> GetAdventureWorksEmployeesAsync();

        Task<MemoryStream> GetEmployeePhoto(int employeeID);

        Task<ResponseAwEmployee> GetEmployee(int employeeID);
    }
}
