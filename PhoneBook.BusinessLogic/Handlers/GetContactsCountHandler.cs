using PhoneBook.Utilities;
using Microsoft.IdentityModel.Tokens;
using Academits.Karetskas.PhoneBook.UnitOfWork.UnitOfWork;
using Academits.Karetskas.PhoneBook.UnitOfWork.Repositories.Interfaces;

namespace Academits.Karetskas.PhoneBook.BusinessLogic.Handlers
{
    public class GetContactsCountHandler
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetContactsCountHandler(IUnitOfWork unitOfWork)
        {
            ExceptionHandling.CheckArgumentForNull(unitOfWork);

            _unitOfWork = unitOfWork;
        }

        public int Handle(string? searchFilterText)
        {
            var filterText = searchFilterText.IsNullOrEmpty() ? "" : searchFilterText;

            return _unitOfWork.GetRepository<IContactRepository>().GetContactsCount(filterText);
        }
    }
}
