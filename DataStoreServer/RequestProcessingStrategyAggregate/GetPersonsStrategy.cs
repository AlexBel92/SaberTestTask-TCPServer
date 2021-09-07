using DataStoreServer.Factories;
using System.Text.Json;
using System.Threading.Tasks;

namespace DataStoreServer
{
    public class GetPersonsStrategy : RequestProcessingStrategy
    {
        public GetPersonsStrategy(IRepositoryFactory repositoryFactory) 
            : base(repositoryFactory)
        {
        }

        public async override Task<string> ExecuteAsync(string request)
        {
            using var repository = repositoryFactory.GetRepository();

            var persons = await repository.GetPersonsAsync();
            return JsonSerializer.Serialize(persons);
        }
    }
}
