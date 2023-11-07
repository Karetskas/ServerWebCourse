using System.Linq;
using PhoneBook.Utilities;
using Microsoft.EntityFrameworkCore;
using Academits.Karetskas.PhoneBook.DataAccess;
using Academits.Karetskas.PhoneBook.DataAccess.Model;
using Academits.Karetskas.PhoneBook.UnitOfWork.Repositories.Interfaces;

namespace Academits.Karetskas.PhoneBook.UnitOfWork.Repositories
{
    public class PhoneNumberRepository : BaseEfRepository<PhoneNumber>, IPhoneNumberRepository
    {
        private readonly DbContext _dbContext;

        public PhoneNumberRepository(PhoneBookDbContext dbContext) : base(dbContext)
        {
            ExceptionHandling.CheckArgumentForNull(dbContext);

            _dbContext = dbContext;
        }

        public bool HasPhoneNumberInContacts(string phone)
        {
            return _dbContext.Set<PhoneNumber>()
                .AsNoTracking()
                .Any(phoneNumber => phoneNumber.Phone == phone);
        }
    }
}
