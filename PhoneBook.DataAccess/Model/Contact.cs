using System.Collections.Generic;

namespace Academits.Karetskas.PhoneBook.DataAccess.Model
{
    public class Contact
    {
        public int Id { get; set; }

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string? SecondName { get; set; }

        public virtual ICollection<PhoneNumber> PhoneNumbers { get; set; } = new List<PhoneNumber>();
    }
}
