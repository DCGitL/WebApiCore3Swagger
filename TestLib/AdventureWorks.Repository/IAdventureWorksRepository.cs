using System.Collections.Generic;
using System.Threading.Tasks;
using TestLib.ResponseModels;

namespace TestLib.AdventureWorks.Repository
{
    public interface IAdventureWorksRepository
    {
       Task<IEnumerable<ResponseAwEmployee>> GetAdventureWorksEmployeesAsync();
    }
}
