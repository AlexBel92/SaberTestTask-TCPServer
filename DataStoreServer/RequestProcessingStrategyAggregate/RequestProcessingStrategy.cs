using DataStore.Core;
using DataStore.Core.Exceptions;
using DataStoreServer.Factories;
using System.Linq;
using System.Threading.Tasks;

namespace DataStoreServer
{
    public abstract class RequestProcessingStrategy
    {
        internal readonly IRepositoryFactory repositoryFactory;

        public RequestProcessingStrategy(IRepositoryFactory repositoryFactory)
        {
            this.repositoryFactory = repositoryFactory;
        }

        public abstract Task<string> ExecuteAsync(string request);

        internal static void ValidatePersonAndThrowOnError(Person person)
        {
            var errors = person.Validate();
            if (errors.Count > 0)
            {
                throw new PersonValidationException(errors.Aggregate((a, b) => a + "\n" + b));
            }
        }
    }
}
