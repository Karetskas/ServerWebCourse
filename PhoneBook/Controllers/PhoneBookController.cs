using PhoneBook.Utilities;
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
        [HttpGet]
        public List<ContactDto> GetContacts([FromServices] GetContactsHandler getContactsHandler, [FromQuery] string? searchFilterText, [FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            ExceptionHandling.CheckArgumentForNull(getContactsHandler);

            return getContactsHandler.Handle(searchFilterText, pageNumber, pageSize);
        }

        [HttpGet]
        public int GetContactsCount([FromServices] GetContactsCountHandler getContactsCountHandler, [FromQuery] string? searchFilterText)
        {
            ExceptionHandling.CheckArgumentForNull(getContactsCountHandler);

            return getContactsCountHandler.Handle(searchFilterText);
        }

        [HttpGet]
        public FileContentResult DownloadExcelFile([FromServices] DownloadExcelFileHandler downloadExcelFileHandler, [FromQuery] string? searchFilterText)
        {
            ExceptionHandling.CheckArgumentForNull(downloadExcelFileHandler);

            var content = downloadExcelFileHandler.Handle(searchFilterText);

            return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "PhoneBool.xlsx");
        }

        [HttpPost]
        public ErrorMessageDto[] AddContact([FromServices] AddContactHandler addContactHandler, ContactDto contact)
        {
            ExceptionHandling.CheckArgumentForNull(addContactHandler);

            return addContactHandler.Handle(contact);
        }

        [HttpPost]
        public void DeleteContacts([FromServices] DeleteContactsHandler deleteContactsHandler, List<int> contactsId)
        {
            ExceptionHandling.CheckArgumentForNull(deleteContactsHandler);

            deleteContactsHandler.Handle(contactsId);
        }
    }
}
