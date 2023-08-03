using Academits.Karetskas.PhoneBook.DataAccess;
using Academits.Karetskas.PhoneBook.Dto;
using Microsoft.EntityFrameworkCore;

namespace PhoneBook.BusinessLogic.Handlers
{
    public class GetContactsHandler
    {
        private readonly PhoneBookDbContext _context;

        public GetContactsHandler(PhoneBookDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context), $"The arguement \"{nameof(context)}\" is null.");
        }

        public List<ContactDto> Handler()
        {
            return _context.Contacts
                .AsNoTracking()
                .Select(contact => new ContactDto
                {
                    Id = contact.Id,
                    FirstName = contact.FirstName,
                    LastName = contact.LastName,
                    SecondName = contact.SecondName,
                    PhoneNumbers = contact.PhoneNumbers
                        .Select(phoneNumber => new PhoneNumberDto
                        {
                            Id = phoneNumber.Id,
                            Phone = phoneNumber.Phone,
                            PhoneType = phoneNumber.PhoneType
                        })
                        .OrderBy(phoneNumberDto => phoneNumberDto.Phone)
                        .ToList()
                })
                .OrderBy(contactDto => contactDto.LastName)
                .ThenBy(contactDto => contactDto.FirstName)
                .ThenBy(contactDto => contactDto.SecondName)
                .ToList();
        }
    }
}
