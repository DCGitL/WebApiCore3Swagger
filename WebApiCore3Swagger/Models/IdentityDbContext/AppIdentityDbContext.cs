using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApiCore3Swagger.Models.RefreshToken.Entity;

namespace WebApiCore3Swagger.Models.IdentityDbContext
{
    public class AppIdentityDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options) : base(options)
        {}
        public DbSet<JwtRefreshToken> JwtRefreshTokens { get; set; }
    }
}
