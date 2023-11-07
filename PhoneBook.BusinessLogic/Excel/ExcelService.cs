using System;
using System.IO;
using System.Linq;
using ClosedXML.Excel;
using PhoneBook.Utilities;
using System.Collections.Generic;
using Academits.Karetskas.PhoneBook.Dto;

namespace Academits.Karetskas.PhoneBook.BusinessLogic.Excel
{
    public sealed class ExcelService : IExcelService
    {
        public void SaveDocument(string path, List<ContactDto> contacts)
        {
            ExceptionHandling.CheckArgumentForNull(path);
            ExceptionHandling.CheckArgumentForNull(contacts);

            using var excelDocument = CreatedDocument(contacts);

            excelDocument.SaveAs(path);
        }

        public byte[] GetDocument(List<ContactDto> contacts)
        {
            ExceptionHandling.CheckArgumentForNull(contacts);

            using var excelDocument = CreatedDocument(contacts);

            using var stream = new MemoryStream();

            excelDocument.SaveAs(stream);

            return stream.ToArray();
        }

        private static XLWorkbook CreatedDocument(List<ContactDto> contacts)
        {
            var excelDocument = new XLWorkbook();

            var workSheet = excelDocument.Worksheets.Add("Phone book");

            const int header = 1;
            var columnsAmount = 1;
            workSheet.Cell(header, columnsAmount).Value = "Last Name";

            columnsAmount++;
            workSheet.Cell(header, columnsAmount).Value = "First Name";

            columnsAmount++;
            workSheet.Cell(header, columnsAmount).Value = "Phone Numbers";

            for (var i = 0; i < contacts.Count; i++)
            {
                columnsAmount = 1;
                workSheet.Cell(i + 2, columnsAmount).Value = contacts[i].LastName;

                columnsAmount++;
                workSheet.Cell(i + 2, columnsAmount).Value = contacts[i].FirstName;

                columnsAmount++;
                workSheet.Cell(i + 2, columnsAmount).Value = string.Join(Environment.NewLine, contacts[i].PhoneNumbers
                    .Select(phoneNumber => $"{phoneNumber.PhoneType} - {phoneNumber.Phone}"));
            }

            const int firstRow = 1;
            const int firstColumn = 1;
            const int rowsCount = 3;
            var table = workSheet.Range(firstRow, firstColumn, contacts.Count + 1, rowsCount);
            table.Style
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                .Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                .Border.SetInsideBorder(XLBorderStyleValues.Thin)
                .Border.SetInsideBorderColor(XLColor.Green)
                .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                .Border.SetInsideBorderColor(XLColor.Green);

            var tableTitle = table.FirstRow();
            tableTitle.Style
                .Font.SetBold()
                .Font.SetFontSize(14)
                .Font.SetFontColor(XLColor.White)
                .Fill.SetBackgroundColor(XLColor.Black);

            const int secondRow = 2;
            var tableBody = table.Range(secondRow, firstColumn, contacts.Count + 1, rowsCount);
            tableBody.Style
                .Font.SetItalic()
                .Font.SetFontSize(12)
                .Font.SetFontColor(XLColor.Red)
                .Fill.SetBackgroundColor(XLColor.PastelOrange);

            workSheet.Rows().AdjustToContents();
            workSheet.Columns().AdjustToContents();

            return excelDocument;
        }
    }
}