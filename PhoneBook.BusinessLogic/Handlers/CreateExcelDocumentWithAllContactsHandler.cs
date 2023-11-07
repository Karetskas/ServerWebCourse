using System;
using System.IO;
using PhoneBook.Utilities;
using System.Collections.Generic;
using Academits.Karetskas.PhoneBook.Dto;
using Microsoft.Extensions.DependencyInjection;
using Academits.Karetskas.PhoneBook.DataAccess;
using Academits.Karetskas.PhoneBook.BusinessLogic.Excel;
using Academits.Karetskas.PhoneBook.UnitOfWork.UnitOfWork;
using Academits.Karetskas.PhoneBook.UnitOfWork.Repositories.Interfaces;

namespace Academits.Karetskas.PhoneBook.BusinessLogic.Handlers
{
    public class CreateExcelDocumentWithAllContactsHandler
    {
        private readonly IExcelService _excel;
        private readonly IServiceProvider _serviceProvider;

        public CreateExcelDocumentWithAllContactsHandler(IServiceProvider serviceProvider, IExcelService excel)
        {
            ExceptionHandling.CheckArgumentForNull(excel);
            ExceptionHandling.CheckArgumentForNull(serviceProvider);

            _serviceProvider = serviceProvider;
            _excel = excel;
        }

        public void Handle(object? path)
        {
            if (path is not string absolutePath)
            {
                return;
            }

            var contacts = GetContacts();

            var fileName = $"PhoneBook_{DateTime.Now:yyyyMMddhhmmss}.xlsx";

            var fullPath = Path.Combine(absolutePath, fileName);

            _excel.SaveDocument(fullPath, contacts);
        }

        private List<ContactDto> GetContacts()
        {
            using var serviceScope = _serviceProvider.CreateScope();

            var serviceProvider = serviceScope.ServiceProvider;
            var unitOfWork = new UnitOfWorkPhoneBook(serviceProvider.GetRequiredService<PhoneBookDbContext>(), serviceProvider);

            const string filterText = "";
            const int firstPageNumber = 1;

            var contactsCount = unitOfWork.GetRepository<IContactRepository>().GetContactsCount(filterText);

            return unitOfWork.GetRepository<IContactRepository>().GetContacts(filterText, firstPageNumber, contactsCount);
        }
    }
}
