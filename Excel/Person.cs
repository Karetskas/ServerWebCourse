using System;
using PhoneNumbers;

namespace Academits.Karetskas.Excel
{
    public sealed class Person
    {
        private string? _lastName;
        private string? _firstName;
        private int _age;
        private string? _phoneNumber;

        public string? LastName
        {
            get => _lastName;

            set
            {
                CheckStringArgument(value);

                _lastName = value;
            }
        }

        public string? FirstName
        {
            get => _firstName;

            set
            {
                CheckStringArgument(value);

                _firstName = value;
            }
        }

        public int Age
        {
            get => _age;

            set
            {
                CheckIntegerArgument(value);

                _age = value;
            }
        }

        public string? PhoneNumber
        {
            get => _phoneNumber;

            set
            {
                var phoneNumberUtil = PhoneNumberUtil.GetInstance();

                try
                {
                    _ = phoneNumberUtil.Parse(value, null);
                }
                catch (NumberParseException)
                {
                    _phoneNumber = null;

                    return;
                }

                _phoneNumber = value;
            }
        }

        public Person(string lastName, string firstName, int age, string phoneNumber)
        {
            LastName = lastName;
            FirstName = firstName;
            Age = age;
            PhoneNumber = phoneNumber;
        }

        private static void CheckStringArgument(string? text)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentNullException(nameof(text), $"Argument \"{nameof(text)}\" is null or empty.");
            }
        }

        private static void CheckIntegerArgument(int number)
        {
            if (number <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(number),
                    $"Argument \"{nameof(number)}\" = {number} out of range. The argument must be greater than 0.");
            }
        }
    }
}
