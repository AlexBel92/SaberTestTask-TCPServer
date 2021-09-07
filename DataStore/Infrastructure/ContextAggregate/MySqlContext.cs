using Microsoft.EntityFrameworkCore;

namespace DataStore.Infrastructure
{
    public class MySqlContext : StorageContext
    {
        public MySqlContext(DbContextOptions options) : base(options)
        {
            Database.Migrate();
        }
    }
}
