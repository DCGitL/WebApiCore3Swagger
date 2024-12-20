using Adventure.Works._2012.dbContext.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiXuinitTest.Helper
{
    public static class SeedDbExtension
    {

        public static void Seed(this NorthwindContext northwindContext)
        {
            var listEmployees = new List<Employees>
            {
                new  Employees
                {
                    FirstName = "David",
                    LastName = "Chen",
                    Address = "65 John street",
                    City = "Toronto"
                },
                 new  Employees
                {
                    FirstName = "David",
                    LastName = "Chen",
                    Address = "65 John street",
                    City = "Toronto"
                }, new  Employees
                {
                    FirstName = "Zeena",
                    LastName = "Tutunji",
                    Address = "650 Don Mill Road",
                    City = "Toronto"
                }
                };

            northwindContext.Employees.AddRange(listEmployees);
            northwindContext.SaveChanges();
        }
    }
}
