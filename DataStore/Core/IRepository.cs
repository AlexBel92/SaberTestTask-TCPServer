using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataStore.Core
{
    public interface IRepository : IDisposable
    {
        public Task<IEnumerable<Person>> GetPersonsAsync();
        public Task<int> AddPersonAsync(Person person);
        public Task UpdatePersonAsync(Person person);
    }
}
