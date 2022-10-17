using Microsoft.EntityFrameworkCore;

namespace MvcProject.Entities
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions options) : base(options)
        {
            //new lemek istemiyorum dependcy ile newlesin program.cs de
        }

        public DbSet<User> Users { get; set; }
    }
}
