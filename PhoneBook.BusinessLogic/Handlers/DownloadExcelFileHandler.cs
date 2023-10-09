using System;
using System.IO;
using System.Linq;
using OfficeOpenXml;
using System.Drawing;
using OfficeOpenXml.Style;
using Microsoft.IdentityModel.Tokens;
using Academits.Karetskas.PhoneBook.UnitOfWork.UnitOfWork;
using Academits.Karetskas.PhoneBook.UnitOfWork.Repositories.Interfaces;

namespace Academits.Karetskas.PhoneBook.BusinessLogic.Handlers
{
    public class DownloadExcelFileHandler
    {
        private readonly IUnitOfWork _unitOfWork;

        public DownloadExcelFileHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork), $"The argument \"{nameof(unitOfWork)}\" is null.");
        }

        public byte[] Handler(string? searchFilterText)
        {
            var filterText = searchFilterText.IsNullOrEmpty() ? "" : searchFilterText;
            var contactsCount = _unitOfWork.GetRepository<IContactRepository>()!.GetContactsCount(filterText);
            var contacts = _unitOfWork.GetRepository<IContactRepository>()!.GetContacts(filterText, 1, contactsCount);

            using var excelPackage = new ExcelPackage();

            excelPackage.Workbook.Properties.Title = "Phone book";
            excelPackage.Workbook.Properties.Subject = "Table of people";

            var worksheet = excelPackage.Workbook.Worksheets.Add("Phone book");

            SetColumns(worksheet, 4);

            worksheet.Cells.Style.Font.Size = 14;

            var columnsNumber = 1;
            SetCell(worksheet, 1, columnsNumber, "Last name", Color.Black, Color.Green, Color.White);
            columnsNumber++;
            SetCell(worksheet, 1, columnsNumber, "First name", Color.Black, Color.Green, Color.White);
            columnsNumber++;
            SetCell(worksheet, 1, columnsNumber, "Phone numbers", Color.Black, Color.Green, Color.White);

            for (var i = 0; i < contacts.Count; i++)
            {
                worksheet.Row(i + 2).Height = worksheet.Cells.Style.Font.Size * 3;

                columnsNumber = 1;
                SetCell(worksheet, i + 2, columnsNumber, contacts[i].LastName, Color.Bisque, Color.Green, Color.Black);

                columnsNumber++;
                SetCell(worksheet, i + 2, columnsNumber, contacts[i].FirstName, Color.Bisque, Color.Green, Color.Black);

                var phoneNumbers = string.Join(Environment.NewLine, contacts[i].PhoneNumbers
                    .Select(phoneNumber => $"{phoneNumber.PhoneType} - {phoneNumber.Phone}"));

                columnsNumber++;
                SetCell(worksheet, i + 2, columnsNumber, phoneNumbers, Color.Bisque, Color.Green, Color.Black);
            }

            worksheet.Cells.AutoFitColumns();

            using var stream = new MemoryStream();

            excelPackage.SaveAs(stream);

            var content = stream.ToArray();

            return content;
        }

        private static void SetColumns(ExcelWorksheet worksheet, int column)
        {
            for (var i = 1; i <= column; i++)
            {
                worksheet.Column(i).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Column(i).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            }
        }

        private static void SetCell<T>(ExcelWorksheet worksheet, int row, int column, T value, Color cellColor, Color borderColor, Color textColor)
        {
            worksheet.Cells[row, column].Value = value;

            worksheet.Cells[row, column].Style.Border.BorderAround(ExcelBorderStyle.Thin, borderColor);

            worksheet.Cells[row, column].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells[row, column].Style.Fill.BackgroundColor.SetColor(cellColor);

            worksheet.Cells[row, column].Style.Font.Color.SetColor(textColor);
            worksheet.Cells[row, column].Style.Font.Italic = true;
        }
    }
}
