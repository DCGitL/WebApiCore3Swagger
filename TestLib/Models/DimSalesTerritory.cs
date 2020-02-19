using System;
using System.Collections.Generic;

namespace TestLib.Models
{
    public partial class DimSalesTerritory
    {
        public DimSalesTerritory()
        {
            DimEmployee = new HashSet<DimEmployee>();
        }

        public int SalesTerritoryKey { get; set; }
        public int? SalesTerritoryAlternateKey { get; set; }
        public string SalesTerritoryRegion { get; set; }
        public string SalesTerritoryCountry { get; set; }
        public string SalesTerritoryGroup { get; set; }
        public byte[] SalesTerritoryImage { get; set; }

        public virtual ICollection<DimEmployee> DimEmployee { get; set; }
    }
}
