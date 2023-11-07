using System;
using PhoneBook.Utilities;
using System.Collections.Generic;
using Academits.Karetskas.PhoneBook.UnitOfWork.UnitOfWork;
using Academits.Karetskas.PhoneBook.UnitOfWork.Repositories.Interfaces;

namespace Academits.Karetskas.PhoneBook.BusinessLogic.Handlers
{
    public class DeleteContactsHandler
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteContactsHandler(IUnitOfWork unitOfWork)
        {
            ExceptionHandling.CheckArgumentForNull(unitOfWork);

            _unitOfWork = unitOfWork;
        }

        public void Handle(List<int>? contactsId)
        {
            if (contactsId is null)
            {
                throw new ArgumentNullException(nameof(contactsId), $"The argument \"{nameof(contactsId)}\" is null.");
            }

            var contacts = _unitOfWork.GetRepository<IContactRepository>().FindAllContactsById(contactsId);

            _unitOfWork.GetRepository<IContactRepository>().DeleteRange(contacts);

            _unitOfWork.Save();
        }
    }
}
