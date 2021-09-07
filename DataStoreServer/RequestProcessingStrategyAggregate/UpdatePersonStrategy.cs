using DataStore.Core;
using DataStoreServer.Factories;
using System.Text.Json;
using System.Threading.Tasks;

namespace DataStoreServer
{
    public class UpdatePersonStrategy : RequestProcessingStrategy
    {
        public UpdatePersonStrategy(IRepositoryFactory repositoryFactory) 
            : base(repositoryFactory)
        {
        }

        public async override Task<string> ExecuteAsync(string request)
        {
            var person = JsonSerializer.Deserialize<Person>(request);

            ValidatePersonAndThrowOnError(person);

            using var repository = repositoryFactory.GetRepository();

            await repository.UpdatePersonAsync(person);

            return JsonSerializer.Serialize(person);
        }
    }
}
