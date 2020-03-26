using Adventure.Works._2012.dbContext.ResponseModels;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Adventure.Works._2012.dbContext.Northwind.Repository
{
    public interface INorthwindRepository
    {
        Task<IEnumerable<ResponseEmployee>> GetAllAsyncEmployees();
        Task<ResponseEmployee> GetAsyncEmployee(int id);

        Task<string> GetAllJsonStringEmployeesAsync();

        Task<string> GetAllXmlStringEmployeesAsync();

        Task<string> GetAllCSVStringEmployeesAsync();
        Task<IEnumerable<ResponseOrder>> GetAllOrders();
    }
}
