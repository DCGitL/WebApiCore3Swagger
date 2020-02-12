using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Adventure.Works._2012.dbContext.Models
{
    public partial class SalesTotalsByAmount
    {
        [Column(TypeName = "money")]
        public decimal? SaleAmount { get; set; }
        [Column("OrderID")]
        public int OrderId { get; set; }
        [Required]
        [StringLength(40)]
        public string CompanyName { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ShippedDate { get; set; }
    }
}
