using System;
using System.Collections.Generic;

namespace TestLib.Models
{
    public partial class DimScenario
    {
        public DimScenario()
        {
            FactFinance = new HashSet<FactFinance>();
        }

        public int ScenarioKey { get; set; }
        public string ScenarioName { get; set; }

        public virtual ICollection<FactFinance> FactFinance { get; set; }
    }
}
