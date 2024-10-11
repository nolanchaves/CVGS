using CVGS.Entities;
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
    }
}
