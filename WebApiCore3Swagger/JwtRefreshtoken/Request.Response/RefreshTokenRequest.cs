using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiCore3Swagger.JwtRefreshtoken.Request.Response
{
    public class RefreshTokenRequest
    {
        /// <summary>
        /// This is the Jwt Token
        /// </summary>
        [Required(ErrorMessage = "A Token is requird for this field")]
        public string Token { get; set; }

        /// <summary>
        /// This token ties to this Jwt token
        /// </summary>
        [Required(ErrorMessage = "A Refresh Token is requird for this field")]
        public string RefreshToken { get; set; }
    }
}
