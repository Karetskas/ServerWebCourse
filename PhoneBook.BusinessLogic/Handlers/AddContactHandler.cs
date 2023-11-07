using System;
using System.Linq;
using PhoneBook.Utilities;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Academits.Karetskas.PhoneBook.Dto;
using Academits.Karetskas.PhoneBook.DataAccess.Model;
using Academits.Karetskas.PhoneBook.UnitOfWork.UnitOfWork;
using Academits.Karetskas.PhoneBook.UnitOfWork.Repositories.Interfaces;

namespace Academits.Karetskas.PhoneBook.BusinessLogic.Handlers
{
    public class AddContactHandler
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddContactHandler(IUnitOfWork unitOfWork)
        {
            ExceptionHandling.CheckArgumentForNull(unitOfWork);

            _unitOfWork = unitOfWork;
        }

        public ErrorMessageDto[] Handle(ContactDto contact)
        {
            var errorMessages = new ErrorMessageDto[]
            {
                new() {FieldType = TextFieldType.FirstName, Message = "" },
                new() {FieldType = TextFieldType.LastName, Message = "" },
                new() {FieldType = TextFieldType.HomePhone, Message = "" },
                new() {FieldType = TextFieldType.WorkPhone, Message = "" },
                new() {FieldType = TextFieldType.MobilePhone, Message = "" }
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

                _unitOfWork.GetRepository<IContactRepository>().AddRange(new Contact
                {
                    FirstName = contact.FirstName,
                    LastName = contact.LastName,
                    PhoneNumbers = contacts
                });

                _unitOfWork.Save();
            }

            return errorMessages;
        }

        private bool AreValidContactFields(ContactDto? contact, ErrorMessageDto[]? errorMessages)
        {
            ExceptionHandling.CheckArgumentForNull(contact);
            ExceptionHandling.CheckArgumentForNull(errorMessages);

            var hasErrorMessages = true;

            foreach (var errorMessage in errorMessages!)
            {
                var hasError = true;

                switch (errorMessage.FieldType)
                {
                    case TextFieldType.FirstName:
                        (hasError, errorMessage.Message) = IsValidTextField(contact?.FirstName);
                        break;
                    case TextFieldType.LastName:
                        (hasError, errorMessage.Message) = IsValidTextField(contact?.LastName);
                        break;
                    case TextFieldType.HomePhone:
                        var homePhone = contact?.PhoneNumbers.FirstOrDefault(phoneNumber => phoneNumber.PhoneType == PhoneNumberType.Home)?.Phone;
                        (hasError, errorMessage.Message) = IsValidPhoneNumberField(homePhone);
                        break;
                    case TextFieldType.WorkPhone:
                        var workPhone = contact?.PhoneNumbers.FirstOrDefault(phoneNumber => phoneNumber.PhoneType == PhoneNumberType.Work)?.Phone;
                        (hasError, errorMessage.Message) = IsValidPhoneNumberField(workPhone);
                        break;
                    case TextFieldType.MobilePhone:
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

        private static (bool, string) IsValidTextField(string? text)
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

            var hasNumber = _unitOfWork.GetRepository<IPhoneNumberRepository>()
                .HasPhoneNumberInContacts(text);

            if (hasNumber)
            {
                return (false, $"The phone number \"{text}\" already exists.");
            }

            return (true, "");
        }
    }
}
