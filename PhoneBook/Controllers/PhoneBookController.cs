using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Academits.Karetskas.PhoneBook.Dto;
using Academits.Karetskas.PhoneBook.BusinessLogic.Handlers;

namespace Academits.Karetskas.PhoneBook.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class PhoneBookController : ControllerBase
    {
        private readonly GetContactsHandler _getContactsHandler;
        private readonly AddContactHandler _addContactHandler;
        private readonly GetContactsCountHandler _getContactsCountHandler;
        private readonly DeleteContactsHandler _deleteContactsHandler;
        private readonly DownloadExcelFileHandler _downloadExcelFileHandler;

        public PhoneBookController(GetContactsHandler getContactsHandler, AddContactHandler addContactHandler, GetContactsCountHandler getContactsCountHandler,
            DeleteContactsHandler deleteContactsHandler, DownloadExcelFileHandler downloadExcelFileHandler)
        {
            _getContactsHandler = getContactsHandler ?? throw new ArgumentNullException(nameof(getContactsHandler),
                $"The argument \"{nameof(getContactsHandler)}\" is null.");

            _addContactHandler = addContactHandler ?? throw new ArgumentNullException(nameof(addContactHandler),
                $"The argument \"{nameof(getContactsHandler)}\" is null.");

            _getContactsCountHandler = getContactsCountHandler ?? throw new ArgumentNullException(nameof(getContactsCountHandler),
                $"The argument \"{nameof(getContactsCountHandler)}\" is null.");

            _deleteContactsHandler = deleteContactsHandler ?? throw new ArgumentNullException(nameof(deleteContactsHandler),
                $"The argument \"{nameof(deleteContactsHandler)}\" is null.");

            _downloadExcelFileHandler = downloadExcelFileHandler ?? throw new ArgumentNullException(nameof(downloadExcelFileHandler),
                $"The argument \"{nameof(downloadExcelFileHandler)}\" is null.");
        }

        [HttpGet]
        public List<ContactDto> GetContacts([FromQuery] string? searchFilterText, [FromQuery] int pageNumber, [FromQuery] int rowsCount)
        {
            return _getContactsHandler.Handler(searchFilterText, pageNumber, rowsCount);
        }

        [HttpGet]
        public int GetContactsCount([FromQuery] string? searchFilterText)
        {
            return _getContactsCountHandler.Handler(searchFilterText);
        }

        [HttpGet]
        public FileContentResult DownloadExcelFile([FromQuery] string? searchFilterText)
        {
            var content = _downloadExcelFileHandler.Handler(searchFilterText);

            return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "PhoneBool.xlsx");
        }

        [HttpPost]
        public ErrorMessageDto[] AddContact(ContactDto contact)
        {
            return _addContactHandler.Handler(contact);
        }

        [HttpPost]
        public void DeleteContacts(List<int> contactsId)
        {
            _deleteContactsHandler.Handler(contactsId);
        }
    }
}
