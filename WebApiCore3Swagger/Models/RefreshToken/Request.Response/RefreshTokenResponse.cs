using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiCore3Swagger.Models.RefreshToken.Request.Response
{
    public class RefreshTokenResponse
    {
        /// <summary>
        /// This is the Jwt token
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// This refresh token is assign to this Jwt token
        /// </summary>
        public string RefreshToken { get; set; }

        /// <summary>
        /// True if a new jwt token was successfully generated
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Error message produce when generating the jwt refresh token if empty most likely the token was successfully generated
        /// </summary>
        public IEnumerable<string> Errors { get; set; }

        public DateTime AccessTokenExpiration { get; set; }

        public DateTime IssuedDate { get; set; }

    }
}
