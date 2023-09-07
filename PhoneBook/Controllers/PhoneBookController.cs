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

        public PhoneBookController(GetContactsHandler getContactsHandler)
        {
            _getContactsHandler = getContactsHandler ?? throw new ArgumentNullException(nameof(getContactsHandler),
                $"The argument \"{nameof(getContactsHandler)}\" is null.");
        }

        public List<ContactDto> GetContacts()
        {
            return _getContactsHandler.Handler();
        }
    }
}
