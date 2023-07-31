namespace Academits.Karetskas.PhoneBook.DataAccess.Model
{
    public class PhoneNumber
    {
        public int Id { get; set; }

        public string Phone { get; set; } = null!;

        public PhoneNumberType PhoneType { get; set; }

        public int ContactId { get; set; }

        public virtual Contact Contact { get; set; } = new();
    }
}
