namespace TechExpoWorld.Data
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    public class TechExpoDbContext : IdentityDbContext
    {
        public TechExpoDbContext(DbContextOptions<TechExpoDbContext> options)
            : base(options)
        {
        }
    }
}
