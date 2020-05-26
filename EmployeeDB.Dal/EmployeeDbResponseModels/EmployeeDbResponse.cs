using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EmployeeDB.Dal.EmployeeDbResponseModels
{
    public  class EmployeeDbResponse
    {
        public int Id { get; set; }

       
        [Display(Name ="First Name")]
        [Required]
        public string FirstName { get; set; }
        [Display(Name ="Last Name")]
        [Required]
        public string LastName { get; set; }
        public string Gender { get; set; }
        public decimal? Salary { get; set; }
    }
}
