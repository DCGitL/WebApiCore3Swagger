using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Adventure.Works._2012.dbContext.Models
{
    public partial class CategorySalesFor1997
    {
        [Required]
        [StringLength(15)]
        public string CategoryName { get; set; }
        [Column(TypeName = "money")]
        public decimal? CategorySales { get; set; }
    }
}
