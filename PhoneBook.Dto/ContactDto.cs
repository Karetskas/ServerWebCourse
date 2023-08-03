using System.Collections.Generic;

namespace Academits.Karetskas.PhoneBook.Dto
{
    public class ContactDto
    {
        public int Id { get; set; }

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string? SecondName { get; set; }

        public List<PhoneNumberDto> PhoneNumbers { get; set; } = new();
    }
}
