using System;
using System.Collections.Generic;

namespace EmployeeDB.Dal.Models
{
    public partial class Employees
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public decimal? Salary { get; set; }
    }
}
