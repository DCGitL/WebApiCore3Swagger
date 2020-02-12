using Adventure.Works._2012.dbContext.Models;
using Adventure.Works._2012.dbContext.ResponseModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Adventure.Works._2012.dbContext.Northwind.Repository
{
    public class NorthwindRepository : INorthwindRepository
    {
        private readonly NorthwindContext context;

        public NorthwindRepository(NorthwindContext context)
        {
            this.context = context;
        }
        public async Task<IEnumerable<ResponseEmployee>> GetAllAsyncEmployees()
        {
            var results = await Task.Run(() =>
            {
                var val = context.Employees.Select(e => new ResponseEmployee
                {
                    EmployeeId = e.EmployeeId,
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    Address = e.Address,
                    PostalCode = e.PostalCode,
                    City = e.City,
                    Country = e.Country,
                    HomePhone = e.HomePhone,
                });

                return val;
            });
           

            return results;
        }

        public async Task<ResponseEmployee> GetAsyncEmployee(int id)
        {
            var result = await Task.Run(() =>
            {
                var val = context.Employees.Select(e=> new ResponseEmployee
                {
                    EmployeeId = e.EmployeeId,
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    Address = e.Address,
                    PostalCode = e.PostalCode,
                    City = e.City,
                    Country = e.Country,
                    HomePhone = e.HomePhone
                } ).FirstOrDefault(e => e.EmployeeId == id);
              
                return val;
              
            });


            return result;
        }
    }
}
