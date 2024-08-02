using Adventure.Works._2012.dbContext.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiXuinitTest.Helper
{
    public static class NorthwindDbExtension
    {

        public static void Seed(this NorthwindContext context)
        {
            if (!context.Employees.Any())
            {
                context.Employees.AddRange(
                    new Employees { Address="6 Lake Shore Road", FirstName="Mary", LastName="Jane",City="Toronto", PostalCode="M3T9A9",Country="Canada"},
                    new Employees { Address ="89 John Street", FirstName ="Zeena", LastName="TuTunji",City="North York", PostalCode="M3T1A5", Country="Canada" });

                context.SaveChanges();
            }
        }
    }
}
