using Academits.Karetskas.PhoneBook.DataAccess.Model;

namespace Academits.Karetskas.PhoneBook.Dto
{
    public class PhoneNumberDto
    {
        public int Id { get; set; }

        public string Phone { get; set; } = null!;

        public string PhoneType { get; set; } = null!;
    }
}
