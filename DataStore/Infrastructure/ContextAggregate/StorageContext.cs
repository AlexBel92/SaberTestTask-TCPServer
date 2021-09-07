using Microsoft.EntityFrameworkCore;

namespace DataStore.Infrastructure
{
    public abstract class StorageContext : DbContext
    {
        public DbSet<EfPerson> Persons { get; set; }

        public StorageContext()
        {            
        }

        public StorageContext(DbContextOptions options) : base(options)
        {            
        }

        protected sealed override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<EfPerson>().ToTable("People");

            modelBuilder
                .Entity<EfPerson>()
                .HasIndex(p => new { p.FirstName, p.MiddleName, p.LastName, p.DateBirth })
                .IsUnique();
        }
    }
}