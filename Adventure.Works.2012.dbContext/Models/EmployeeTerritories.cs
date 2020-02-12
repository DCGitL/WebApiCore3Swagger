using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Adventure.Works._2012.dbContext.Models
{
    public partial class EmployeeTerritories
    {
        [Key]
        [Column("EmployeeID")]
        public int EmployeeId { get; set; }
        [Key]
        [Column("TerritoryID")]
        [StringLength(20)]
        public string TerritoryId { get; set; }

        [ForeignKey(nameof(EmployeeId))]
        [InverseProperty(nameof(Employees.EmployeeTerritories))]
        public virtual Employees Employee { get; set; }
        [ForeignKey(nameof(TerritoryId))]
        [InverseProperty(nameof(Territories.EmployeeTerritories))]
        public virtual Territories Territory { get; set; }
    }
}
