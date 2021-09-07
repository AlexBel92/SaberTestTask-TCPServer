using DataStore.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace DataStoreServer.Factories
{
    public interface IStorageContextFactory
    {
        public StorageContext GetStorageContext();
    }

    public class MySqlContextFactory : IStorageContextFactory
    {
        private readonly string connectionString;

        public MySqlContextFactory(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new System.ArgumentException($"'{nameof(connectionString)}' cannot be null or whitespace.", nameof(connectionString));
            }

            this.connectionString = connectionString;
        }

        public StorageContext GetStorageContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<MySqlContext>();
            optionsBuilder.UseMySQL(connectionString);
            return new MySqlContext(optionsBuilder.Options);
        }
    }

    public class InMemoryContextFactory : IStorageContextFactory
    {
        public StorageContext GetStorageContext()
        {
            return new InMemoryContext();
        }
    }
}