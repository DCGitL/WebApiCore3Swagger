using System;
using System.Collections.Generic;
using System.Text;

namespace Adventure.Works._2012.dbContext.ResponseModels
{
    public class ResponseOrder
    {
        public int OrderId { get; set; }
        
        public DateTime? OrderDate { get; set; }
       
        public DateTime? RequiredDate { get; set; }
        
        public DateTime? ShippedDate { get; set; }

        public decimal? Freight { get; set; }
       
        public string ShipName { get; set; }
      
        public string ShipAddress { get; set; }
      
        public string ShipCity { get; set; }
      
        public string ShipRegion { get; set; }
      
        public string ShipPostalCode { get; set; }
      
        public string ShipCountry { get; set; }

    }
}
