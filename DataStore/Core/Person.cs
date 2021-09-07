using System;
using System.Collections.Generic;

namespace DataStore.Core
{
    public class Person
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public DateTime DateBirth { get; set; }

        public List<string> Validate()
        {
            var errors = new List<string>();

            ValidateName(errors);

            if (DateBirth.Date > DateTime.Now.Date)
            {
                errors.Add($"Field: {nameof(DateBirth)}. Date cannot be from the future.");
            }

            return errors;
        }

        private void ValidateName(List<string> errors)
        {
            var fullName = new Dictionary<string, string>()
            {
                { nameof(FirstName), FirstName },
                { nameof(MiddleName), MiddleName },
                { nameof(LastName), LastName }
            };

            foreach (var partOfName in fullName)
            {
                var error = ValidatePartOfName(partOfName.Value);
                if (!string.IsNullOrEmpty(error))
                    errors.Add($"Field: {partOfName.Key}. " + error);

            }
        }

        private string ValidatePartOfName(string partOfName)
        {
            var result = string.Empty;

            if (string.IsNullOrWhiteSpace(partOfName))
            {
                result = $"String cannot be null or white spaces.";
            }
            else if (FirstName.Length > 50)
            {
                result = $"String cannot be longer than 50 characters.";
            }

            return result;
        }
    }
}