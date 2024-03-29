﻿using System.Collections.Generic;
using Academits.Karetskas.PhoneBook.Dto;
using Academits.Karetskas.PhoneBook.DataAccess.Model;

namespace Academits.Karetskas.PhoneBook.UnitOfWork.Repositories.Interfaces
{
    public interface IContactRepository : IGenericRepository<Contact>
    {
        List<ContactDto> GetContacts(string? searchFilterText, int pageNumber, int rowsCount);

        int GetContactsCount(string? searchFilterText);

        Contact[] FindAllContactsById(List<int> contactsId);
    }
}
