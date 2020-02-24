using System;
using System.Collections.Generic;

namespace TestLib.Models
{
    public partial class DimOrganization
    {
        public DimOrganization()
        {
            FactFinance = new HashSet<FactFinance>();
            InverseParentOrganizationKeyNavigation = new HashSet<DimOrganization>();
        }

        public int OrganizationKey { get; set; }
        public int? ParentOrganizationKey { get; set; }
        public string PercentageOfOwnership { get; set; }
        public string OrganizationName { get; set; }
        public int? CurrencyKey { get; set; }

        public virtual DimCurrency CurrencyKeyNavigation { get; set; }
        public virtual DimOrganization ParentOrganizationKeyNavigation { get; set; }
        public virtual ICollection<FactFinance> FactFinance { get; set; }
        public virtual ICollection<DimOrganization> InverseParentOrganizationKeyNavigation { get; set; }
    }
}
