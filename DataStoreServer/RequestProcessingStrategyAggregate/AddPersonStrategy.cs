using DataStore.Core;
using DataStoreServer.Factories;
using System.Text.Json;
using System.Threading.Tasks;

namespace DataStoreServer
{
    public class AddPersonStrategy : RequestProcessingStrategy
    {
        public AddPersonStrategy(IRepositoryFactory repositoryFactory) 
            : base(repositoryFactory)
        {
        }

        public async override Task<string> ExecuteAsync(string request)
        {
            var person = JsonSerializer.Deserialize<Person>(request);
            person.Id = 0;

            ValidatePersonAndThrowOnError(person);

            using var repository = repositoryFactory.GetRepository();

            var addedId = await repository.AddPersonAsync(person);

            return addedId.ToString();
        }        
    }
}
