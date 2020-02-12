using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiCore3Swagger.Models.IdentityDbContext
{
    public class ApplicationUser : IdentityUser
    {
        public string City { get; set; }
    }
}
