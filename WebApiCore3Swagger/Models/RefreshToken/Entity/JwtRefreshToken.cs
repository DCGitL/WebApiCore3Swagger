using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using WebApiCore3Swagger.Models.IdentityDbContext;

namespace WebApiCore3Swagger.Models.RefreshToken.Entity
{
    public class JwtRefreshToken
    {
        [Key]
        public string RefreshToken { get; set; }

        public string JwtId { get; set; }
        public string UserId { get; set; }

        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Long live date 2 months 
        /// </summary>
        public DateTime ExpiryDate { get; set; }


        public bool Used { get; set; }

        public bool Invalidated { get; set; }

        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; }

    }
}
