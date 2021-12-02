using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using WebApiCore3Swagger.Models.Auth;
using WebApiCore3Swagger.Models.IdentityDbContext;
using WebApiCore3Swagger.Models.RefreshToken.Entity;
using WebApiCore3Swagger.Models.RefreshToken.Request.Response;

namespace WebApiCore3Swagger.Services.Auth
{
    public class AuthJwtService : IAuthJwtService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IOptions<AppSetting> appConfigSettings;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly AppIdentityDbContext appIdentityDbContext;
        private DateTime TokenExpiration = DateTime.UtcNow;
        private DateTime IssuedDate = DateTime.UtcNow;
        public AuthJwtService(UserManager<ApplicationUser> userManager, IOptions<AppSetting> appConfigSettings, TokenValidationParameters tokenValidationParameters, AppIdentityDbContext appIdentityDbContext)
        {
            this.userManager = userManager;
            this.appConfigSettings = appConfigSettings;
            this._tokenValidationParameters = tokenValidationParameters;
            this.appIdentityDbContext = appIdentityDbContext;
        }
        public async Task<ResponseAuth> AsyncAuthenticate(string userName, string password)
        {
            try
            {
               
                var appUser = await userManager.FindByNameAsync(userName);
                if (appUser == null && !await userManager.CheckPasswordAsync(appUser, password))
                {
                    return null;
                }

                
                var tokenResult = await CreateJwtTokenAsync(appUser);

                return tokenResult;


            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw ex;
            }
 
          
        }


        private async Task<ResponseAuth> CreateJwtTokenAsync(ApplicationUser appUser)
        {
            ResponseAuth responseAuth = null;

            var appUserRoles = await userManager.GetRolesAsync(appUser);

            var tokenHandler = new JwtSecurityTokenHandler();
            var certificate = new X509Certificate2(appConfigSettings.Value.PrivateKeyLocation, "abc12345");


            var key =  new X509SecurityKey(certificate) ; //Encoding.ASCII.GetBytes(appConfigSettings.Value.JwtTokenSecret);

           
                DateTime today = DateTime.UtcNow;
                TimeSpan duration = appConfigSettings.Value.TokenLifeTime;
                TokenExpiration = today.Add(duration);
            

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, appUser.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, GenerateJwtTokenId()),
                new Claim(ClaimTypes.Name, appUser.UserName),
                new Claim(JwtRegisteredClaimNames.Email, appUser.Email),
              //  new Claim(ClaimTypes.Email, appUser.Email),
                new Claim("id", appUser.Id)
            };

            if(appUserRoles != null && appUserRoles.Count > 0)
            {
                foreach(var role in appUserRoles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims, "Bearer"),
                NotBefore = DateTime.UtcNow.AddMinutes(-1),
                IssuedAt = IssuedDate,
                Expires = TokenExpiration,
                Audience = "http://my.audience.com",
                Issuer = "http://tokenissuer.com",
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.RsaSha384Signature)     //(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };

            var token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);

            responseAuth = new ResponseAuth
            {
                AccessToken = tokenHandler.WriteToken(token),
                ExpirationDateTime = TokenExpiration,
                DateIssued = IssuedDate
            };

            var _refreshToken = new JwtRefreshToken
            {
                RefreshToken = Guid.NewGuid().ToString(),
                JwtId = token.Id,
                UserId = appUser.Id,
                CreatedDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddMonths(6)

            };

            try
            {
                await appIdentityDbContext.JwtRefreshTokens.AddAsync(_refreshToken);
                await appIdentityDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                var msg = ex.Message;

                throw ex;
            }
            responseAuth.RefreshToken = _refreshToken.RefreshToken;
            return responseAuth ;
        }

        public async Task<RefreshTokenResponse> RefreshTokenAsyc(string token, string refreshToken)
        {
            RefreshTokenResponse refreshTokenResult = null;

            var validatedToken = GetPrincipalFromToken(token);
            if (validatedToken == null)
            {
                //token is invalid
                return new RefreshTokenResponse { Errors = new[] { "Invalid Token " } };
            }
            _tokenValidationParameters.ValidateLifetime = true;

            var exptime = validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value;

            var expiryDateUnix = long.Parse(exptime);

            var expiryDateTimeUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                                       .AddSeconds(expiryDateUnix);


            if (expiryDateTimeUtc > DateTime.UtcNow)
            {
                return new RefreshTokenResponse { Errors = new[] { "This token is not yet expired" } };
            }

            //get id from the jwt token
            var jti = validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

            JwtRefreshToken storedRefreshToken = null;
            try
            {
                 storedRefreshToken =  appIdentityDbContext.JwtRefreshTokens.FirstOrDefault(r => r.RefreshToken == refreshToken);

                if (storedRefreshToken == null)
                {
                    return new RefreshTokenResponse { Errors = new[] { "This refresh token does not exist" } };
                }
            }
            catch(Exception ex)
            {
                var msg = ex.Message;
                throw ex;

            }
          

            if (DateTime.UtcNow > storedRefreshToken.ExpiryDate)
            {
                return new RefreshTokenResponse { Errors = new[] { "This refresh token has expired" } };
            }

            if (storedRefreshToken.Invalidated)
            {
                return new RefreshTokenResponse { Errors = new[] { "This refresh token has been invalidated" } };
            }

            if (storedRefreshToken.Used)
            {
                return new RefreshTokenResponse { Errors = new[] { "This refresh token has been used" } };
            }

            if (storedRefreshToken.JwtId != jti)
            {
                return new RefreshTokenResponse { Errors = new[] { "This refresh token does not match this JWT" } };
            }

            storedRefreshToken.Used = true;
            appIdentityDbContext.JwtRefreshTokens.Update(storedRefreshToken);
            await appIdentityDbContext.SaveChangesAsync();

            //get the user id from the token
            var userId = validatedToken.Claims.Single(x => x.Type == "id").Value;
            var user = await userManager.FindByIdAsync(userId);


            var generateRefreshToken = await CreateJwtTokenAsync(user);

            refreshTokenResult = new RefreshTokenResponse
            {
                Token = generateRefreshToken.AccessToken,
                RefreshToken = generateRefreshToken.RefreshToken,
                Success = true,
                AccessTokenExpiration = generateRefreshToken.ExpirationDateTime, 
                IssuedDate = IssuedDate
            };

            return refreshTokenResult;

        }

        private string GenerateJwtTokenId()
        {
            var randomnumber = new byte[32];
          
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomnumber);

                return Convert.ToBase64String(randomnumber);
            }
        }


        private ClaimsPrincipal GetPrincipalFromToken(string token)
        {

            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                _tokenValidationParameters.ValidateLifetime = false;
                var principal = tokenHandler.ValidateToken(token, _tokenValidationParameters, out var validatedToken);
                if (!IsJWtWithValidSecurityAlgorithm(validatedToken))
                {
                    return null;
                }

                return principal;
            }
            catch
            {
                return null;
            }

        }


        private bool IsJWtWithValidSecurityAlgorithm(SecurityToken validatedToken)
        {
            return (validatedToken is JwtSecurityToken jwtSecurityToken) && jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.RsaSha384, StringComparison.InvariantCultureIgnoreCase);
        }

    }
}
