using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiCore3Swagger.Models.Account
{
    public class ResponseUser
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string City { get; set; }

        public List<string> Roles { get; set; }
    }
}
