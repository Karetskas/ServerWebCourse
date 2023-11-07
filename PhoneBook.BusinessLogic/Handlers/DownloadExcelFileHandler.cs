using PhoneBook.Utilities;
using Academits.Karetskas.PhoneBook.BusinessLogic.Excel;
using Academits.Karetskas.PhoneBook.UnitOfWork.UnitOfWork;
using Academits.Karetskas.PhoneBook.UnitOfWork.Repositories.Interfaces;

namespace Academits.Karetskas.PhoneBook.BusinessLogic.Handlers
{
    public class DownloadExcelFileHandler
    {
        private readonly IUnitOfWork _unitOfWork;

        public DownloadExcelFileHandler(IUnitOfWork unitOfWork)
        {
            ExceptionHandling.CheckArgumentForNull(unitOfWork);

            _unitOfWork = unitOfWork;
        }

        public byte[] Handle(string? searchFilterText)
        {
            var filterText = searchFilterText ?? "";
            var contactsCount = _unitOfWork.GetRepository<IContactRepository>().GetContactsCount(filterText);
            var contacts = _unitOfWork.GetRepository<IContactRepository>().GetContacts(filterText, 1, contactsCount);

            var excel = new ExcelService();

            return excel.GetDocument(contacts);
        }
    }
}
