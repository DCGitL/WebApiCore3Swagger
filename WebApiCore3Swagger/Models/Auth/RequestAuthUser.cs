using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiCore3Swagger.Models.Auth
{
    public class RequestAuthUser
    {
        [Required(ErrorMessage ="UserName is a required field")]
        public string userName { get; set; }

        [Required(ErrorMessage = "Password is a required field")]
        public string Password { get; set; }
    }
}
