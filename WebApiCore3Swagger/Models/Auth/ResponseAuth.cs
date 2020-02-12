using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiCore3Swagger.Models.Auth
{
    public class ResponseAuth
    {
        public string AccessToken { get; set; }
        public DateTime ExpirationDateTime { get; set; }

        public DateTime DateIssued { get; set; }

        public string RefreshToken { get; set; }
    }
}
