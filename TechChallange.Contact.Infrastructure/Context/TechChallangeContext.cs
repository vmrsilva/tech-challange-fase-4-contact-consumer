using Microsoft.EntityFrameworkCore;
using TechChallange.Contact.Domain.Contact.Entity;

namespace TechChallange.Contact.Infrastructure.Context
{
    public class TechChallangeContext : DbContext
    {
        public TechChallangeContext() : base()
        {
            // Database.Migrate();
        }

        public TechChallangeContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {
        }
        public DbSet<ContactEntity> Contact { get; set; }
    }
}
