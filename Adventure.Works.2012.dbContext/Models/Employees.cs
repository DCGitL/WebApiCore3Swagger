using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Adventure.Works._2012.dbContext.Models
{
    public partial class Employees
    {
        public Employees()
        {
            EmployeeTerritories = new HashSet<EmployeeTerritories>();
            InverseReportsToNavigation = new HashSet<Employees>();
            Orders = new HashSet<Orders>();
        }

        [Key]
        [Column("EmployeeID")]
        public int EmployeeId { get; set; }
        [Required]
        [StringLength(20)]
        public string LastName { get; set; }
        [Required]
        [StringLength(10)]
        public string FirstName { get; set; }
        [StringLength(30)]
        public string Title { get; set; }
        [StringLength(25)]
        public string TitleOfCourtesy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? BirthDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? HireDate { get; set; }
        [StringLength(60)]
        public string Address { get; set; }
        [StringLength(15)]
        public string City { get; set; }
        [StringLength(15)]
        public string Region { get; set; }
        [StringLength(10)]
        public string PostalCode { get; set; }
        [StringLength(15)]
        public string Country { get; set; }
        [StringLength(24)]
        public string HomePhone { get; set; }
        [StringLength(4)]
        public string Extension { get; set; }
        [Column(TypeName = "image")]
        public byte[] Photo { get; set; }
        [Column(TypeName = "ntext")]
        public string Notes { get; set; }
        public int? ReportsTo { get; set; }
        [StringLength(255)]
        public string PhotoPath { get; set; }

        [ForeignKey(nameof(ReportsTo))]
        [InverseProperty(nameof(Employees.InverseReportsToNavigation))]
        public virtual Employees ReportsToNavigation { get; set; }
        [InverseProperty("Employee")]
        public virtual ICollection<EmployeeTerritories> EmployeeTerritories { get; set; }
        [InverseProperty(nameof(Employees.ReportsToNavigation))]
        public virtual ICollection<Employees> InverseReportsToNavigation { get; set; }
        [InverseProperty("Employee")]
        public virtual ICollection<Orders> Orders { get; set; }
    }
}
