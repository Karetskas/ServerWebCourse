using System;
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
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork), $"The argument \"{nameof(unitOfWork)}\" is null.");
        }

        public List<ContactDto> Handler(string? searchFilterText, int pageNumber, int rowsCount)
        {
            var filterText = searchFilterText.IsNullOrEmpty() ? "" : searchFilterText;

            var contactsCount = _unitOfWork.GetRepository<IContactRepository>()!.GetContactsCount(filterText);

            if (rowsCount < 1)
            {
                return new List<ContactDto>();
            }

            var pagesCount = (int)Math.Ceiling((decimal)contactsCount / rowsCount);

            if (pageNumber < 1 || pageNumber > pagesCount)
            {
                return new List<ContactDto>();
            }

            return _unitOfWork.GetRepository<IContactRepository>()!.GetContacts(filterText, pageNumber, rowsCount);
        }
    }
}
