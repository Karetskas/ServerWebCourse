using System;
using System.Collections.Generic;
using Academits.Karetskas.PhoneBook.UnitOfWork.Repositories.Interfaces;
using Academits.Karetskas.PhoneBook.UnitOfWork.UnitOfWork;

namespace Academits.Karetskas.PhoneBook.BusinessLogic.Handlers
{
    public class DeleteContactsHandler
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteContactsHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork), $"The argument \"{nameof(unitOfWork)}\" is null.");
        }

        public void Handler(List<int>? contactsId)
        {
            if (contactsId is null)
            {
                throw new ArgumentNullException(nameof(contactsId), $"The argument \"{nameof(contactsId)}\" is null.");
            }

            var contacts = _unitOfWork.GetRepository<IContactRepository>()!.FindAllContactsById(contactsId);

            _unitOfWork.GetRepository<IContactRepository>()!.DeleteRange(contacts);

            _unitOfWork.Save();
        }
    }
}
