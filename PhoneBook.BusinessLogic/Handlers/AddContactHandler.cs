using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using Academits.Karetskas.PhoneBook.Dto;
using Academits.Karetskas.PhoneBook.DataAccess;
using Academits.Karetskas.PhoneBook.DataAccess.Model;
using Academits.Karetskas.PhoneBook.UnitOfWork.Repositories.Interfaces;
using Academits.Karetskas.PhoneBook.UnitOfWork.UnitOfWork;

namespace Academits.Karetskas.PhoneBook.BusinessLogic.Handlers
{
    public class AddContactHandler
    {
        //private readonly PhoneBookDbContext _context;

        //public AddContactHandler(PhoneBookDbContext context)
        //{
        //    _context = context ?? throw new ArgumentNullException(nameof(context), $"The argument \"{nameof(context)}\" is null.");
        //}

        private readonly IUnitOfWork _unitOfWork;

        public AddContactHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork), $"The argument \"{nameof(unitOfWork)}\" is null.");
        }

        public ErrorMessageDto[] Handler(ContactDto contact)
        {
            var errorMessages = new ErrorMessageDto[]
            {
                new() {FieldType = TextFieldTypeDto.FirstName, Message = "" },
                new() {FieldType = TextFieldTypeDto.LastName, Message = "" },
                new() {FieldType = TextFieldTypeDto.HomePhone, Message = "" },
                new() {FieldType = TextFieldTypeDto.WorkPhone, Message = "" },
                new() {FieldType = TextFieldTypeDto.MobilePhone, Message = "" }
            };

            if (AreValidContactFields(contact, errorMessages))
            {
                var contacts = new List<PhoneNumber>();

                contact.PhoneNumbers.ForEach(phoneNumber =>
                {
                    contacts.Add(new PhoneNumber
                    {
                        Phone = phoneNumber.Phone,
                        PhoneType = phoneNumber.PhoneType
                    });
                });

                _unitOfWork.GetRepository<IContactRepository>()!.AddRange(new Contact
                {
                    FirstName = contact.FirstName,
                    LastName = contact.LastName,
                    PhoneNumbers = contacts
                });

                _unitOfWork.Save();

                //_context.Contacts.Add(new Contact
                //{
                //    FirstName = contact.FirstName,
                //    LastName = contact.LastName,
                //    PhoneNumbers = contacts
                //});

                //_context.SaveChanges();
            }

            return errorMessages;
        }

        private bool AreValidContactFields(ContactDto? contact, ErrorMessageDto[]? errorMessages)
        {
            CheckArgument(contact);
            CheckArgument(errorMessages);
            
            var hasErrorMessages = true;

            foreach (var errorMessage in errorMessages!)
            {
                var hasError = true;

                switch (errorMessage.FieldType)
                {
                    case TextFieldTypeDto.FirstName:
                        (hasError, errorMessage.Message) = IsValidTextField(contact?.FirstName);
                        break;
                    case TextFieldTypeDto.LastName:
                        (hasError, errorMessage.Message) = IsValidTextField(contact?.LastName);
                        break;
                    case TextFieldTypeDto.HomePhone:
                        var homePhone = contact?.PhoneNumbers.FirstOrDefault(phoneNumber => phoneNumber.PhoneType == PhoneNumberType.Home)?.Phone;
                        (hasError, errorMessage.Message) = IsValidPhoneNumberField(homePhone);
                        break;
                    case TextFieldTypeDto.WorkPhone:
                        var workPhone = contact?.PhoneNumbers.FirstOrDefault(phoneNumber => phoneNumber.PhoneType == PhoneNumberType.Work)?.Phone;
                        (hasError, errorMessage.Message) = IsValidPhoneNumberField(workPhone);
                        break;
                    case TextFieldTypeDto.MobilePhone:
                        var mobilePhone = contact?.PhoneNumbers.FirstOrDefault(phoneNumber => phoneNumber.PhoneType == PhoneNumberType.Mobile)?.Phone;
                        (hasError, errorMessage.Message) = IsValidPhoneNumberField(mobilePhone);
                        break;
                    default:
                        throw new InvalidOperationException($"Unknown text field type \"{nameof(errorMessage.FieldType)}\" = {errorMessage.FieldType}.");
                }

                hasErrorMessages &= hasError;
            }

            return hasErrorMessages;
        }

        private (bool, string) IsValidTextField(string? text)
        {
            if (text is null)
            {
                return (true, "");
            }

            if (text.Length == 0)
            {
                return (false, "Required field!");
            }

            if (text.Length > 255)
            {
                return (false, "Text longer than 255 characters!");
            }

            if (text.StartsWith(" "))
            {
                return (false, "Space at the beginning of the line!");
            }

            if (text.EndsWith(" "))
            {
                return (false, "Space at the end of the line!");
            }

            return (true, "");
        }

        private (bool, string) IsValidPhoneNumberField(string? text)
        {
            if (text is null)
            {
                return (true, "");
            }

            (bool isValidMessage, string ErrorMessage) validationResult = IsValidTextField(text);

            if (!validationResult.isValidMessage)
            {
                return validationResult;
            }

            if (!Regex.IsMatch(text, @"[0-9+]"))
            {
                return (false, "The phone number must contain at least 1 digit!");
            }

            if (Regex.IsMatch(text, @"[^0-9-+.() ]"))
            {
                return (false, "Allowed characters: (, ), +, -, 0-9, dots and spaces!");
            }

            var hasNumber = _unitOfWork.GetRepository<IPhoneNumberRepository>()!
                .HasPhoneNumberInContacts(text);

            //var hasNumber = _context.PhoneNumbers
            //    .AsNoTracking()
            //    .Any(phoneNumber => phoneNumber.Phone == text);

            if (hasNumber)
            {
                return (false, $"The phone number \"{text}\" already exists.");
            }

            return (true, "");
        }

        private void CheckArgument(object? obj)
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj), $"The argument \"{nameof(obj)}\" is null.");
            }
        }
    }
}
