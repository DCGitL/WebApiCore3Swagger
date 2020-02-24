using System;
using System.Collections.Generic;

namespace TestLib.Models
{
    public partial class DimCurrency
    {
        public DimCurrency()
        {
            DimOrganization = new HashSet<DimOrganization>();
        }

        public int CurrencyKey { get; set; }
        public string CurrencyAlternateKey { get; set; }
        public string CurrencyName { get; set; }

        public virtual ICollection<DimOrganization> DimOrganization { get; set; }
    }
}
