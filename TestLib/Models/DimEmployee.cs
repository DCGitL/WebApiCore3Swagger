using System;
using System.Collections.Generic;

namespace TestLib.Models
{
    public partial class DimEmployee
    {
        public DimEmployee()
        {
            InverseParentEmployeeKeyNavigation = new HashSet<DimEmployee>();
        }

        public int EmployeeKey { get; set; }
        public int? ParentEmployeeKey { get; set; }
        public string EmployeeNationalIdalternateKey { get; set; }
        public string ParentEmployeeNationalIdalternateKey { get; set; }
        public int? SalesTerritoryKey { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public bool NameStyle { get; set; }
        public string Title { get; set; }
        public DateTime? HireDate { get; set; }
        public DateTime? BirthDate { get; set; }
        public string LoginId { get; set; }
        public string EmailAddress { get; set; }
        public string Phone { get; set; }
        public string MaritalStatus { get; set; }
        public string EmergencyContactName { get; set; }
        public string EmergencyContactPhone { get; set; }
        public bool? SalariedFlag { get; set; }
        public string Gender { get; set; }
        public byte? PayFrequency { get; set; }
        public decimal? BaseRate { get; set; }
        public short? VacationHours { get; set; }
        public short? SickLeaveHours { get; set; }
        public bool CurrentFlag { get; set; }
        public bool SalesPersonFlag { get; set; }
        public string DepartmentName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Status { get; set; }
        public byte[] EmployeePhoto { get; set; }

        public virtual DimEmployee ParentEmployeeKeyNavigation { get; set; }
        public virtual DimSalesTerritory SalesTerritoryKeyNavigation { get; set; }
        public virtual ICollection<DimEmployee> InverseParentEmployeeKeyNavigation { get; set; }
    }
}
