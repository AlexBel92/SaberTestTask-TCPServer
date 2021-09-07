using DataStore.Core;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using MySql.Data.MySqlClient;
using DataStore.Core.Exceptions;

namespace DataStore.Infrastructure
{
    public class EfRepository : IRepository
    {
        private readonly StorageContext storageContext;

        public EfRepository(StorageContext storageContext)
        {
            this.storageContext = storageContext;
        }

        public async Task<IEnumerable<Person>> GetPersonsAsync()
        {
            var entityPersons = await storageContext.Persons.OrderByDescending(p => p.Id).ToListAsync();

            return entityPersons.Select(ep => (Person)ep);
        }

        public async Task<int> AddPersonAsync(Person person)
        {
            var entityPerson = CreateEntityPersonFrom(person);

            await storageContext.Persons.AddAsync(entityPerson);
            await SaveChangesAsync();

            return entityPerson.Id;
        }

        private static EfPerson CreateEntityPersonFrom(Person person)
        {
            return (EfPerson)person;
        }

        private async Task SaveChangesAsync()
        {
            try
            {
                await storageContext.SaveChangesAsync();
            }
            catch (DbUpdateException e) when (e.InnerException is MySqlException mySqlException && mySqlException.Number == 1062)
            {                
                throw new DuplicateKeyException($"Such person already exists.", e);
            }
        }

        public async Task UpdatePersonAsync(Person person)
        {
            var dbEntryPerson = await storageContext.Persons.FirstOrDefaultAsync(p => p.Id == person.Id);

            if (dbEntryPerson is null)
            {
                throw new PersonNotFoundException($"Person with id = {person.Id} not found.");
            }

            dbEntryPerson.FirstName = person.FirstName;
            dbEntryPerson.MiddleName = person.MiddleName;
            dbEntryPerson.LastName = person.LastName;
            dbEntryPerson.DateBirth = person.DateBirth;

            await SaveChangesAsync();
        }

        public void Dispose()
        {
            storageContext.Dispose();
        }
    }
}
