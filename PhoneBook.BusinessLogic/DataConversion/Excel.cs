using System;
using System.IO;
using System.Linq;
using OfficeOpenXml;
using System.Drawing;
using OfficeOpenXml.Style;
using System.Collections.Generic;
using Academits.Karetskas.PhoneBook.Dto;

namespace Academits.Karetskas.PhoneBook.BusinessLogic.DataConversion
{
    public sealed class Excel : IExcel
    {
        public void SaveDocument(string path, List<ContactDto> contacts)
        {
            CheckArgument(path);
            CheckArgument(contacts);

            using var excelPackage = CreatedDocument(contacts);

            var fileInfo = new FileInfo(path);
            excelPackage.SaveAs(fileInfo);
        }

        public byte[] GetDocument(List<ContactDto> contacts)
        {
            CheckArgument(contacts);

            using var excelPackage = CreatedDocument(contacts);

            using var stream = new MemoryStream();

            excelPackage.SaveAs(stream);

            return stream.ToArray();
        }

        private static void CheckArgument(object? obj)
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj), $"The argument {nameof(obj)} is null.");
            }
        }

        private static ExcelPackage CreatedDocument(List<ContactDto> contacts)
        {
            var excelPackage = new ExcelPackage();

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

            return excelPackage;
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
