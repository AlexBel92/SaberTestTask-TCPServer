using Microsoft.EntityFrameworkCore;

namespace DataStore.Infrastructure
{
    public class InMemoryContext : StorageContext
    {
        public InMemoryContext() : base()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("testDB");
        }
    }
}
