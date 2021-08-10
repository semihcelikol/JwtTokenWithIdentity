using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JwtTokenWithIdentity.Models
{
    public class APIDbContext : IdentityDbContext<ApplicationUser>
    {
        public readonly IHttpContextAccessor httpContextAccessor;

        public APIDbContext(DbContextOptions<APIDbContext> options, IHttpContextAccessor httpContextAccessor)
            : base(options)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public DbSet<ApplicationUserTokens> ApplicationUserTokens { get; set; }
        public DbSet<Products> Products { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Products>().ToTable("Products");
            builder.Entity<Products>().HasIndex(x => x.Name).IsUnique();
        }

    }
}
