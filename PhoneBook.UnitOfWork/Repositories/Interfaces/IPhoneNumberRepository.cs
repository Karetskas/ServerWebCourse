using Academits.Karetskas.PhoneBook.DataAccess.Model;

namespace Academits.Karetskas.PhoneBook.UnitOfWork.Repositories.Interfaces
{
    public interface IPhoneNumberRepository : IGenericRepository<PhoneNumber>
    {
        bool HasPhoneNumberInContacts(string phone);
    }
}
