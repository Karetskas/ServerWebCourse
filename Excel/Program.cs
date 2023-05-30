using System.IO;
using OfficeOpenXml;
using System.Drawing;
using OfficeOpenXml.Style;
using System.Collections.Generic;

namespace Academits.Karetskas.Excel
{
    internal class Program
    {
        public static void Main()
        {
            var people = new List<Person>(10)
            {
                new Person("Ivanov", "Ivan", 17, "+79083456534"),
                new Person("Petrov", "Petr", 71, "+79023453396"),
                new Person("Sidorov", "Petr", 33, "+7(908) 376-64-34"),
                new Person("Danilov", "Denis", 21, "+79434340404"),
                new Person("Ogurov", "Grisha", 88, "+79033436534"),
                new Person("Besstrashnay", "Masha", 14, "+7 905 345-65-35"),
                new Person("Kupidonov", "Roma", 42, "+79033446544"),
                new Person("Strogov", "Semen", 17, "+79883856584"),
                new Person("Bugaev", "Vasia", 28, "+79683466564"),
                new Person("Burgerova", "Klava", 64, "+79093496539")
            };

            using var excelPackage = new ExcelPackage();

            excelPackage.Workbook.Properties.Title = "Phone book";
            excelPackage.Workbook.Properties.Subject = "Table of people";

            var worksheet = excelPackage.Workbook.Worksheets.Add("Phone book");

            SetColumns(worksheet, 4);

            var columnsNumber = 1;
            SetCell(worksheet, 1, columnsNumber, "Last name", Color.Black, Color.Green, Color.White);
            columnsNumber++;
            SetCell(worksheet, 1, columnsNumber, "First name", Color.Black, Color.Green, Color.White);
            columnsNumber++;
            SetCell(worksheet, 1, columnsNumber, "Age", Color.Black, Color.Green, Color.White);
            columnsNumber++;
            SetCell(worksheet, 1, columnsNumber, "Phone number", Color.Black, Color.Green, Color.White);

            for (var i = 0; i < people.Count; i++)
            {
                columnsNumber = 1;
                SetCell(worksheet, i + 2, columnsNumber, people[i].LastName, Color.Bisque, Color.Green, Color.Black);

                columnsNumber++;
                SetCell(worksheet, i + 2, columnsNumber, people[i].FirstName, Color.Bisque, Color.Green, Color.Black);

                columnsNumber++;
                SetCell(worksheet, i + 2, columnsNumber, people[i].Age.ToString(), Color.Bisque, Color.Green, Color.Black);

                columnsNumber++;
                SetCell(worksheet, i + 2, columnsNumber, people[i].PhoneNumber, Color.Bisque, Color.Green, Color.Black);
            }

            worksheet.Cells.Style.Font.Size = 14;
            worksheet.Cells.AutoFitColumns();

            var fileInfo = new FileInfo(@"PhoneBook.xlsx");
            excelPackage.SaveAs(fileInfo);
        }

        private static void SetColumns(ExcelWorksheet worksheet, int column)
        {
            for (var i = 1; i <= column; i++)
            {
                worksheet.Column(i).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Column(i).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            }
        }

        private static void SetCell(ExcelWorksheet worksheet, int row, int column, string? value, Color cellColor, Color borderColor, Color textColor)
        {
            value ??= "null";

            worksheet.Cells[row, column].Value = value;

            worksheet.Cells[row, column].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            worksheet.Cells[row, column].Style.Border.Bottom.Color.SetColor(borderColor);

            worksheet.Cells[row, column].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            worksheet.Cells[row, column].Style.Border.Left.Color.SetColor(borderColor);

            worksheet.Cells[row, column].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            worksheet.Cells[row, column].Style.Border.Right.Color.SetColor(borderColor);

            worksheet.Cells[row, column].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            worksheet.Cells[row, column].Style.Border.Top.Color.SetColor(borderColor);

            worksheet.Cells[row, column].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells[row, column].Style.Fill.BackgroundColor.SetColor(cellColor);

            worksheet.Cells[row, column].Style.Font.Color.SetColor(textColor);
            worksheet.Cells[row, column].Style.Font.Italic = true;
        }
    }
}