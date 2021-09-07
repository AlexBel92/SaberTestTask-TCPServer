using System;

namespace DataStore.Core.Exceptions
{
    public class PersonNotFoundException : Exception
    {
        public PersonNotFoundException()
        {
        }

        public PersonNotFoundException(string message)
            : base(message)
        {
        }

        public PersonNotFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
