using System.Collections.Generic;
using Academits.Karetskas.PhoneBook.Dto;

namespace Academits.Karetskas.PhoneBook.BusinessLogic.Excel;

public interface IExcelService
{
    void SaveDocument(string path, List<ContactDto> contacts);

    byte[] GetDocument(List<ContactDto> contacts);
}