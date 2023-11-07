using System;

namespace PhoneBook.Utilities
{
    public static class ExceptionHandling
    {
        public static void CheckArgumentForNull(object? obj)
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj), $"The argument \"{nameof(obj)}\" is null.");
            }
        }
    }
}