using System;
using Microsoft.IdentityModel.Tokens;
using Academits.Karetskas.PhoneBook.UnitOfWork.UnitOfWork;
using Academits.Karetskas.PhoneBook.BusinessLogic.DataConversion;
using Academits.Karetskas.PhoneBook.UnitOfWork.Repositories.Interfaces;

namespace Academits.Karetskas.PhoneBook.BusinessLogic.Handlers
{
    public class DownloadExcelFileHandler
    {
        private readonly IUnitOfWork _unitOfWork;

        public DownloadExcelFileHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork), $"The argument \"{nameof(unitOfWork)}\" is null.");
        }

        public byte[] Handler(string? searchFilterText)
        {
            var filterText = searchFilterText.IsNullOrEmpty() ? "" : searchFilterText;
            var contactsCount = _unitOfWork.GetRepository<IContactRepository>()!.GetContactsCount(filterText);
            var contacts = _unitOfWork.GetRepository<IContactRepository>()!.GetContacts(filterText, 1, contactsCount);

            var excel = new Excel();

            return excel.GetDocument(contacts);
        }
    }
}
