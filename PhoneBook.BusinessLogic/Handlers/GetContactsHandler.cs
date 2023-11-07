using System;
using PhoneBook.Utilities;
using System.Collections.Generic;
using Microsoft.IdentityModel.Tokens;
using Academits.Karetskas.PhoneBook.Dto;
using Academits.Karetskas.PhoneBook.UnitOfWork.UnitOfWork;
using Academits.Karetskas.PhoneBook.UnitOfWork.Repositories.Interfaces;

namespace Academits.Karetskas.PhoneBook.BusinessLogic.Handlers
{
    public class GetContactsHandler
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetContactsHandler(IUnitOfWork unitOfWork)
        {
            ExceptionHandling.CheckArgumentForNull(unitOfWork);

            _unitOfWork = unitOfWork;
        }

        public List<ContactDto> Handle(string? searchFilterText, int pageNumber, int rowsCount)
        {
            var filterText = searchFilterText.IsNullOrEmpty() ? "" : searchFilterText;

            var contactsCount = _unitOfWork.GetRepository<IContactRepository>().GetContactsCount(filterText);

            if (rowsCount < 1)
            {
                return new List<ContactDto>();
            }

            var pagesCount = (int)Math.Ceiling((decimal)contactsCount / rowsCount);

            if (pageNumber < 1 || pageNumber > pagesCount)
            {
                return new List<ContactDto>();
            }

            return _unitOfWork.GetRepository<IContactRepository>().GetContacts(filterText, pageNumber, rowsCount);
        }
    }
}
