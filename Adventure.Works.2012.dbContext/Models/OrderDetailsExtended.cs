﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Adventure.Works._2012.dbContext.Models
{
    public partial class OrderDetailsExtended
    {
        [Column("OrderID")]
        public int OrderId { get; set; }
        [Column("ProductID")]
        public int ProductId { get; set; }
        [Required]
        [StringLength(40)]
        public string ProductName { get; set; }
        [Column(TypeName = "money")]
        public decimal UnitPrice { get; set; }
        public short Quantity { get; set; }
        public float Discount { get; set; }
        [Column(TypeName = "money")]
        public decimal? ExtendedPrice { get; set; }
    }
}
