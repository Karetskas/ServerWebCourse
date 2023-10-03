using Academits.Karetskas.PhoneBook.UnitOfWork.UnitOfWork;
using System;
using Academits.Karetskas.PhoneBook.UnitOfWork.Repositories.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace Academits.Karetskas.PhoneBook.BusinessLogic.Handlers
{
    public class GetContactsCountHandler
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetContactsCountHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork), $"The argument \"{nameof(unitOfWork)}\" is null.");
        }

        public int Handler(string? searchFilterText)
        {
            var filterText = searchFilterText.IsNullOrEmpty() ? "" : searchFilterText;

            return _unitOfWork.GetRepository<IContactRepository>()!.GetContactsCount(filterText);
        }
    }
}
