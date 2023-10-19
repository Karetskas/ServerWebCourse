using System.Collections.Generic;
using Academits.Karetskas.PhoneBook.Dto;

namespace Academits.Karetskas.PhoneBook.BusinessLogic.DataConversion;

public interface IExcel
{
    void SaveDocument(string path, List<ContactDto> contacts);

    byte[] GetDocument(List<ContactDto> contacts);
}