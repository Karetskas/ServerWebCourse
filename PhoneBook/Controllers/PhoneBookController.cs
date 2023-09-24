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

        public PhoneBookController(GetContactsHandler getContactsHandler, AddContactHandler addContactHandler)
        {
            _getContactsHandler = getContactsHandler ?? throw new ArgumentNullException(nameof(getContactsHandler),
                $"The argument \"{nameof(getContactsHandler)}\" is null.");

            _addContactHandler = addContactHandler ?? throw new ArgumentNullException(nameof(addContactHandler),
                $"The argument \"{nameof(getContactsHandler)}\" is null.");
        }

        [HttpGet]
        public List<ContactDto> GetContacts()
        {
            return _getContactsHandler.Handler();
        }

        [HttpPost]
        public ErrorMessageDto[] AddContact(ContactDto contact)
        {


            return _addContactHandler.Handler(contact);
        }
    }
}
