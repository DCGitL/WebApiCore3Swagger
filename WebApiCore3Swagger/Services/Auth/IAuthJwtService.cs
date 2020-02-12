using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebApiCore3Swagger.Models.Auth;
using WebApiCore3Swagger.Models.RefreshToken.Request.Response;

namespace WebApiCore3Swagger
{
    public interface IAuthJwtService
    {
        Task<ResponseAuth> AsyncAuthenticate(string userName, string password);
        Task<RefreshTokenResponse> RefreshTokenAsyc(string token, string refreshToken);
        
    }
}
