using System;
using System.Data;
using System.Linq;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;

namespace Academits.Karetskas.AdoNet
{
    internal class Program
    {
        static void Main()
        {
            const string connectionString = "Server=Micron;Database=Shop;Encrypt=True;TrustServerCertificate=True;Trusted_Connection=true;";

            using var connection = new SqlConnection(connectionString);
            connection.Open();

            PrintToConsole($"\"Connection state = {connection.State}", ConsoleColor.Yellow, true, true);
            PrintToConsole("1. Вывести общее количество товаров.", ConsoleColor.Blue, true, false);
            PrintToConsole("", ConsoleColor.White, false, true);

            const string productsRequestTotalCount = "SELECT COUNT(*)"
                                        + "FROM Product";

            RunQuery(productsRequestTotalCount, connection, command =>
            {
                var result = Convert.ToInt32(command.ExecuteScalar());
                var message = $"Общее количество товаров = {result}";

                PrintToConsole(message, ConsoleColor.Gray, true, true);
            });

            PrintToConsole("2. Создать некоторую категорию и товар.", ConsoleColor.Blue, true, true);

            const string queryTableProductCategory = "SELECT * FROM ProductCategory";

            PrintTableFromSqlDataReader(connection, "ProductCategory", queryTableProductCategory);
            PrintToConsole("Введите новую категорию продуктового товара: ", ConsoleColor.Gray, false, false);
            var newCategory = Console.ReadLine();

            const string queryToWriteNewCategory = "INSERT INTO ProductCategory(Name)"
                              + "VALUES (@NewCategory)";

            RunQuery(queryToWriteNewCategory, connection, command =>
            {
                command.Parameters.Add(new SqlParameter("@NewCategory", newCategory));

                command.ExecuteNonQuery();
            });

            PrintToConsole("", ConsoleColor.White, false, true);
            PrintTableFromSqlDataReader(connection, "ProductCategory", queryTableProductCategory);

            const string queryTableProduct = "SELECT * FROM Product";

            PrintTableFromSqlDataReader(connection, "Product", queryTableProduct);

            PrintToConsole("Введите следующую информацию о товаре:", ConsoleColor.Gray, true, false);
            PrintToConsole("Название товара: ", ConsoleColor.Gray, false, false);
            var productName = Console.ReadLine();
            PrintToConsole("Стоимость товара: ", ConsoleColor.Gray, false, false);
            _ = decimal.TryParse(Console.ReadLine(), out var productPrice);
            PrintToConsole("Выберите категорию товара из таблицы ниже:", ConsoleColor.Gray, true, false);
            PrintToConsole("", ConsoleColor.White, false, true);
            PrintTableFromSqlDataReader(connection, "ProductCategory", queryTableProductCategory);
            PrintToConsole("Введите категорию товара: ", ConsoleColor.White, false, false);
            var productCategory = Console.ReadLine();

            const string queryToWriteNewProduct = "INSERT INTO Product(Name, Price, CategoryId) "
                                                  + "SELECT @ProductName, @ProductPrice, ProductCategory.Id "
                                                  + "FROM ProductCategory "
                                                  + "WHERE ProductCategory.Name = @ProductCategory";

            RunQuery(queryToWriteNewProduct, connection, command =>
            {
                command.Parameters.Add(new SqlParameter("@ProductName", productName));

                command.Parameters.Add(new SqlParameter("@ProductPrice", productPrice)
                {
                    SqlDbType = SqlDbType.Decimal
                });

                command.Parameters.Add(new SqlParameter("@ProductCategory", productCategory));

                command.ExecuteNonQuery();
            });

            PrintToConsole("", ConsoleColor.White, false, true);
            PrintTableFromSqlDataReader(connection, "Product", queryTableProduct);

            PrintToConsole("3. Отредактировать некоторый товар.", ConsoleColor.Blue, true, true);

            PrintTableFromSqlDataReader(connection, "Product", queryTableProduct);
            PrintToConsole("Введите имя товара который желаете отредактировать из таблицы \"Product\" выше: ",
                ConsoleColor.Gray, false, false);
            var productName1 = Console.ReadLine();

            PrintToConsole("Введите новую стоимость товара: ", ConsoleColor.Gray, false, false);
            _ = decimal.TryParse(Console.ReadLine(), out var productPrice1);

            const string queryToChangeProductPrice = "UPDATE Product "
                                                     + "SET Product.Price = @ProductPrice "
                                                     + "WHERE Product.Name = @ProductName";

            RunQuery(queryToChangeProductPrice, connection, command =>
            {
                command.Parameters.Add(new SqlParameter("@ProductName", productName1));

                command.Parameters.Add(new SqlParameter("@ProductPrice", productPrice1)
                {
                    SqlDbType = SqlDbType.Decimal
                });

                command.ExecuteNonQuery();
            });

            PrintToConsole("", ConsoleColor.White, false, true);
            PrintTableFromSqlDataReader(connection, "Product", queryTableProduct);

            PrintToConsole("4. Удалить некоторый товар.", ConsoleColor.Blue, true, true);
            PrintTableFromSqlDataReader(connection, "Product", queryTableProduct);

            PrintToConsole("Выберите имя товара из таблицы \"Product\" выше: ", ConsoleColor.Gray, false, false);
            var productName2 = Console.ReadLine();

            const string queryToDeleteProduct = "DELETE FROM Product "
                                                + "WHERE Product.Name = @ProductName";

            RunQuery(queryToDeleteProduct, connection, command =>
            {
                command.Parameters.Add(new SqlParameter("@ProductName", productName2));

                command.ExecuteNonQuery();
            });

            PrintToConsole("", ConsoleColor.White, false, true);
            PrintTableFromSqlDataReader(connection, "Product", queryTableProduct);

            PrintToConsole("5. Выгрузить весь список товаров вместе с именами категорий через reader, и распечатайте все данные в цикле.",
                ConsoleColor.Blue, true, true);

            const string queryProductsListWithCategories1 = "SELECT Product.Name Product, ProductCategory.Name Category "
                                                           + "FROM Product "
                                                           + "INNER JOIN ProductCategory "
                                                           + "  ON Product.CategoryId = ProductCategory.Id";

            PrintTableFromSqlDataReader(connection, "Список товаров с категориями", queryProductsListWithCategories1);

            PrintToConsole("6. Выгрузить весь список товаров вместе с именами категорий в DataSet через SqlDataAdapter, и распечатайте все данные в цикле.",
                ConsoleColor.Blue, true, true);

            const string queryProductsListWithCategories2 = "SELECT Product.Name Product, ProductCategory.Name Category "
                                                           + "FROM Product "
                                                           + "INNER JOIN ProductCategory "
                                                           + "  ON Product.CategoryId = ProductCategory.Id";

            PrintTableFromDataSet(connection, "Список товаров с категориями через \"DataSet\"",
                queryProductsListWithCategories2);
        }

        private static void PrintTableFromDataSet(SqlConnection? connection, string? tableName, string query)
        {
            CheckArgument(connection);
            CheckArgument(tableName);
            CheckArgument(query);

            PrintToConsole($"Таблица \"{tableName}\"", ConsoleColor.Gray, true, false);

            try
            {
                using var adapter = new SqlDataAdapter(query, connection);
                var dataSet = new DataSet();
                adapter.Fill(dataSet);

                var columnsCount = dataSet.Tables[0].Columns.Count;
                var maxColumnsWidth = new int[columnsCount];
                var columns = dataSet.Tables[0].Columns;
                var rows = dataSet.Tables[0].Rows;

                for (var i = 0; i < columnsCount; i++)
                {
                    maxColumnsWidth[i] = maxColumnsWidth[i] < columns[i].ColumnName.Length
                        ? columns[i].ColumnName.Length
                        : maxColumnsWidth[i];

                    for (var j = 0; j < rows.Count; j++)
                    {
                        var cells = rows[j].ItemArray;

                        foreach (var cell in cells)
                        {
                            var cellLength = cell?.ToString()?.Length ?? 4;

                            maxColumnsWidth[i] = maxColumnsWidth[i] < cellLength
                                ? cellLength
                                : maxColumnsWidth[i];
                        }
                    }
                }

                PrintTable(maxColumnsWidth, columnsCount, counter =>
                {
                    var spaces = maxColumnsWidth[counter] >= columns[counter].ColumnName.Length
                        ? maxColumnsWidth[counter] - columns[counter].ColumnName.Length
                        : 0;

                    return (spaces, columns[counter].ColumnName);
                }, () =>
                {
                    var cellsResults = new (int spaces, string cellValue)[rows.Count, columnsCount];

                    for (var i = 0; i < columnsCount; i++)
                    {
                        for (var j = 0; j < rows.Count; j++)
                        {
                            var cells = rows[j].ItemArray;

                            for (var k = 0; k < cells.Length; k++)
                            {
                                var cellLength = cells[k]?.ToString()?.Length ?? 4;

                                cellsResults[j, k].spaces = maxColumnsWidth[i] >= cellLength
                                    ? maxColumnsWidth[i] - cellLength
                                    : 0;

                                cellsResults[j, k].cellValue = cells[k]?.ToString() ?? "null";
                            }
                        }
                    }

                    return cellsResults;
                });
            }
            catch (Exception e)
            {
                PrintToConsole(e.ToString(), ConsoleColor.Red, false, false);
            }
        }

        private static void PrintTableFromSqlDataReader(SqlConnection? connection, string? tableName, string query)
        {
            CheckArgument(connection);
            CheckArgument(tableName);
            CheckArgument(query);

            PrintToConsole($"Таблица \"{tableName}\"", ConsoleColor.Gray, true, false);

            RunQuery(query, connection, command =>
            {
                using var reader = command.ExecuteReader();

                var columnsCount = reader.FieldCount;
                var header = new string[columnsCount];
                var maxColumnsWidth = new int[columnsCount];

                for (var i = 0; i < columnsCount; i++)
                {
                    header[i] = reader.GetName(i);

                    if (maxColumnsWidth[i] < header[i].Length)
                    {
                        maxColumnsWidth[i] = header[i].Length;
                    }
                }

                var table = new List<string[]>
                {
                    header
                };

                while (reader.Read())
                {
                    var row = new string[columnsCount];

                    for (var i = 0; i < columnsCount; i++)
                    {
                        row[i] = reader[i].ToString() ?? "null";

                        if (maxColumnsWidth[i] < row[i].Length)
                        {
                            maxColumnsWidth[i] = row[i].Length;
                        }
                    }

                    table.Add(row);
                }

                PrintTable(maxColumnsWidth, columnsCount, counter =>
                {
                    var spaces = maxColumnsWidth[counter] >= table[0][counter].Length
                        ? maxColumnsWidth[counter] - table[0][counter].Length
                        : 0;

                    return (spaces, table[0][counter]);
                }, () =>
                {
                    var cellsResults = new (int spaces, string cellValue)[table.Count - 1, columnsCount];

                    for (var row = 1; row < table.Count; row++)
                    {
                        for (var column = 0; column < columnsCount; column++)
                        {
                            cellsResults[row - 1, column].spaces = maxColumnsWidth[column] >= table[row][column].Length
                                ? maxColumnsWidth[column] - table[row][column].Length
                                : 0;

                            cellsResults[row - 1, column].cellValue = table[row][column];
                        }
                    }

                    return cellsResults;
                });
            });
        }

        private static void PrintTable(int[] maxColumnsWidth, int columnsCount, Func<int, (int spaces, string cellValue)> formHeader, Func<(int spaces, string cellValue)[,]> formBody)
        {
            var allColumnsWidth = maxColumnsWidth.Aggregate((total, column) => total + column);

            PrintToConsole($" {new string('-', allColumnsWidth + columnsCount * 3 + 1)}", ConsoleColor.DarkGreen, true, false);

            for (var i = 0; i < columnsCount; i++)
            {
                var (spaces, cellValue) = formHeader(i);

                PrintToConsole($" | {new string(' ', spaces)}{cellValue}", ConsoleColor.DarkGreen, false, false);
            }

            PrintToConsole(" | ", ConsoleColor.DarkGreen, true, false);
            PrintToConsole($" {new string('-', allColumnsWidth + columnsCount * 3 + 1)}", ConsoleColor.DarkGreen, true, false);

            var cells = formBody();

            for (var i = 0; i < cells.GetLength(0); i++)
            {
                for (var j = 0; j < cells.GetLength(1); j++)
                {
                    PrintToConsole($" | {new string(' ', cells[i, j].spaces)}{cells[i, j].cellValue}", ConsoleColor.DarkGreen, false, false);
                }

                PrintToConsole(" | ", ConsoleColor.DarkGreen, true, false);
            }

            PrintToConsole($" {new string('-', allColumnsWidth + columnsCount * 3 + 1)}", ConsoleColor.DarkGreen, true, false);
            PrintToConsole("", ConsoleColor.White, false, true);
        }

        private static void RunQuery(string? queryText, SqlConnection? connection, Action<SqlCommand> task)
        {
            try
            {
                using var command = new SqlCommand(queryText, connection);

                task(command);
            }
            catch (Exception e)
            {
                PrintToConsole(e.ToString(), ConsoleColor.Red, false, false);
            }
        }

        private static void PrintToConsole(string? message, ConsoleColor color, bool isLineBreakEnabled, bool isAddingEmptyStringEnabled)
        {
            CheckArgument(message);
            CheckArgument(color);

            Console.ForegroundColor = color;

            if (isLineBreakEnabled)
            {
                Console.WriteLine(message);
            }
            else
            {
                Console.Write(message);
            }

            Console.ResetColor();

            if (isAddingEmptyStringEnabled)
            {
                Console.WriteLine();
            }
        }

        private static void CheckArgument(object? obj)
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj), $"The argument \"{nameof(obj)}\" is null.");
            }
        }
    }
}