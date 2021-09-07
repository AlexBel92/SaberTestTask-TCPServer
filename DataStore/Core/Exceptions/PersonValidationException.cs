using System;

namespace DataStore.Core.Exceptions
{
    public class PersonValidationException : Exception
    {
        public PersonValidationException()
        {
        }

        public PersonValidationException(string message)
            : base(message)
        {
        }

        public PersonValidationException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
