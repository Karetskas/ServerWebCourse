using System;
using System.Collections.Generic;
using System.Linq;
using Academits.Karetskas.PhoneBook.DataAccess;
using Academits.Karetskas.PhoneBook.DataAccess.Model;
using Academits.Karetskas.PhoneBook.Dto;
using Academits.Karetskas.PhoneBook.UnitOfWork.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Academits.Karetskas.PhoneBook.UnitOfWork.Repositories
{
    public class ContactRepository : BaseEfRepository<Contact>, IContactRepository
    {
        private readonly DbContext _dbContext;

        public ContactRepository(PhoneBookDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext is null
                ? throw new ArgumentNullException(nameof(dbContext), $"The argument \"{nameof(dbContext)}\" is null.")
                : dbContext;
        }

        public List<ContactDto> GetContacts(string? searchFilterText, int pageNumber, int rowsCount)
        {
            return _dbContext.Set<Contact>()
                .AsNoTracking()
                .Where(contact => EF.Functions.Like(contact.FirstName, $"%{searchFilterText}%")
                                  || EF.Functions.Like(contact.LastName, $"%{searchFilterText}%")
                                  || contact.PhoneNumbers.Any(phoneNumber => EF.Functions.Like(phoneNumber.Phone, $"%{searchFilterText}%")))
                .Skip((pageNumber - 1) * rowsCount)
                .Take(rowsCount)
                .Select(contact => new ContactDto
                {
                    Id = contact.Id,
                    FirstName = contact.FirstName,
                    LastName = contact.LastName,
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
                .ToList();
        }

        public int GetContactsCount(string? searchFilterText)
        {
            return _dbContext
                .Set<Contact>()
                .AsNoTracking()
                .Count(contact => EF.Functions.Like(contact.FirstName, $"%{searchFilterText}%")
                                  || EF.Functions.Like(contact.LastName, $"%{searchFilterText}%")
                                  || contact.PhoneNumbers.Any(phoneNumber =>
                                      EF.Functions.Like(phoneNumber.Phone, $"%{searchFilterText}%")));

            //return _dbContext.Set<Contact>()
            //    .AsNoTracking()
            //    .Count();
        }
    }
}
