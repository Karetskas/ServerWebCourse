using System;
using System.Collections.Generic;
using Academits.Karetskas.PhoneBook.BusinessLogic.Handlers;
using Academits.Karetskas.PhoneBook.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Academits.Karetskas.PhoneBook.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class PhoneBookController : ControllerBase
    {
        private readonly GetContactsHandler _getContactsHandler;
        private readonly AddContactHandler _addContactHandler;
        private readonly GetContactsCountHandler _getContactsCountHandler;

        public PhoneBookController(GetContactsHandler getContactsHandler, AddContactHandler addContactHandler, GetContactsCountHandler getContactsCountHandler)
        {
            _getContactsHandler = getContactsHandler ?? throw new ArgumentNullException(nameof(getContactsHandler),
                $"The argument \"{nameof(getContactsHandler)}\" is null.");

            _addContactHandler = addContactHandler ?? throw new ArgumentNullException(nameof(addContactHandler),
                $"The argument \"{nameof(getContactsHandler)}\" is null.");

            _getContactsCountHandler = getContactsCountHandler ?? throw new ArgumentNullException(nameof(getContactsCountHandler),
                $"The argument \"{nameof(getContactsCountHandler)}\" is null.");
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

        [HttpPost]
        public ErrorMessageDto[] AddContact(ContactDto contact)
        {
            return _addContactHandler.Handler(contact);
        }
    }
}
