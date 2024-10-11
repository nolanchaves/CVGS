using CVGS.Entities;
using CVGS.Entities.CVGS.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace CVGS
{
    public class CvgsDbContext : IdentityDbContext<User>
    {
        public CvgsDbContext(DbContextOptions<CvgsDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // One-to-one relationship between User and Address
            builder.Entity<User>()
                .HasOne(u => u.Address)
                .WithOne(a => a.User)
                .HasForeignKey<Address>(a => a.UserId);

            // One-to-one relationship between User and Preference
            builder.Entity<User>()
                .HasOne(u => u.Preferences)
                .WithOne(p => p.User)
                .HasForeignKey<Preference>(p => p.UserId);
        }

        public DbSet<Preference> Preferences { get; set; }
        public DbSet<Address> Addresses { get; set; }
    }
}
