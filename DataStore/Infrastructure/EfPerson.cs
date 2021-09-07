using DataStore.Core;
using System;
using System.ComponentModel.DataAnnotations;

namespace DataStore.Infrastructure
{
    public class EfPerson
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string MiddleName { get; set; }

        [MaxLength(50)]
        public string LastName { get; set; }

        public DateTime DateBirth { get; set; }

        public static explicit operator Person(EfPerson efPerson)
        {
            return new Person()
            {
                Id = efPerson.Id,
                FirstName = efPerson.FirstName,
                MiddleName = efPerson.MiddleName,
                LastName = efPerson.LastName,
                DateBirth = efPerson.DateBirth
            };
        }

        public static explicit operator EfPerson(Person person)
        {
            return new EfPerson()
            {
                Id = person.Id,
                FirstName = person.FirstName,
                MiddleName = person.MiddleName,
                LastName = person.LastName,
                DateBirth = person.DateBirth
            };
        }
    }
}
