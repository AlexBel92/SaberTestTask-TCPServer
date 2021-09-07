using DataStore.Core;
using DataStore.Infrastructure;

namespace DataStoreServer.Factories
{
    public interface IRepositoryFactory
    {
        IRepository GetRepository();
    }

    public class EfRepositoryFactory : IRepositoryFactory
    {
        private readonly IStorageContextFactory contextFactory;

        public EfRepositoryFactory(IStorageContextFactory contextFactory)
        {
            this.contextFactory = contextFactory;
        }

        public IRepository GetRepository()
        {
            return new EfRepository(contextFactory.GetStorageContext());
        }
    }
}