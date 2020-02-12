using System.ComponentModel.DataAnnotations;

namespace WebApiCore3Swagger.Models.RefreshToken.Request.Response
{
    public class RefreshTokenRequest
    {
        [Required(ErrorMessage ="A Token is required for this field")]
        public string Token { get; set; }
        [Required(ErrorMessage = "A Refresh Token is required for this field")]
        public string RefreshToken { get; set; }
    }
}
