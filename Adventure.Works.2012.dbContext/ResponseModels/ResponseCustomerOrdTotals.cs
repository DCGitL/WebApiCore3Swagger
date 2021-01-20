using System;
using System.Collections.Generic;
using System.Text;

namespace Adventure.Works._2012.dbContext.ResponseModels
{
    public class ResponseCustomerOrdTotals
    {
        public string CustomerID { get; set; }
        public decimal OrderTotal { get; set; }
    }
}
