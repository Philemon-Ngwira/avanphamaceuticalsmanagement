using avanphamaceuticalsmanagement.Shared.IdentityModel;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace avanphamaceuticalsmanagement.Server.Data
{
    public class AdminContext : IdentityDbContext<ApplicationUser>
    {
        public AdminContext(DbContextOptions<AdminContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>(entity =>
            {
                entity.Property(e => e.ProfilePicture)
                    .HasColumnName("ProfilePicture")
                    .HasMaxLength(255); 
            });
        }


    }



}
