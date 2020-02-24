using System;
using System.Collections.Generic;

namespace TestLib.Models
{
    public partial class DimDepartmentGroup
    {
        public DimDepartmentGroup()
        {
            FactFinance = new HashSet<FactFinance>();
            InverseParentDepartmentGroupKeyNavigation = new HashSet<DimDepartmentGroup>();
        }

        public int DepartmentGroupKey { get; set; }
        public int? ParentDepartmentGroupKey { get; set; }
        public string DepartmentGroupName { get; set; }

        public virtual DimDepartmentGroup ParentDepartmentGroupKeyNavigation { get; set; }
        public virtual ICollection<FactFinance> FactFinance { get; set; }
        public virtual ICollection<DimDepartmentGroup> InverseParentDepartmentGroupKeyNavigation { get; set; }
    }
}
