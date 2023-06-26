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
            const string connectionString = "Server=Micron;Database=shop;Encrypt=True;TrustServerCertificate=True;Trusted_Connection=true;";

            using var connection = new SqlConnection(connectionString);
            connection.Open();

            PrintToConsole($"\"Connection state = {connection.State}", ConsoleColor.Yellow, true, true);
            PrintToConsole("1. Вывести общее количество товаров.", ConsoleColor.Blue, true, false);
            PrintToConsole("", ConsoleColor.White, false, true);

            const string goodsRequestTotalCount = "SELECT COUNT(*)"
                                        + "FROM product";

            RunQuery(goodsRequestTotalCount, connection, command =>
            {
                var result = Convert.ToInt32(command.ExecuteScalar());
                var message = $"Общее количество товаров = {result}";

                PrintToConsole(message, ConsoleColor.Gray, true, true);
            });

            PrintToConsole("2. Создать некоторую категорию и товар.", ConsoleColor.Blue, true, true);

            const string queryTableProductCategories = "SELECT * FROM productCategories";

            PrintTableFromSqlDataReader(connection, "productCategories", queryTableProductCategories);
            PrintToConsole("Введите новую категорию продуктового товара: ", ConsoleColor.Gray, false, false);
            var newCategory = Console.ReadLine();

            const string queryToWriteNewCategory = "INSERT INTO productCategories(name)"
                              + "VALUES (@newCategory)";

            RunQuery(queryToWriteNewCategory, connection, command =>
            {
                command.Parameters.Add(new SqlParameter("@newCategory", newCategory)
                {
                    SqlDbType = SqlDbType.NVarChar
                });

                command.ExecuteNonQuery();
            });

            PrintToConsole("", ConsoleColor.White, false, true);
            PrintTableFromSqlDataReader(connection, "productCategories", queryTableProductCategories);

            const string queryTableProduct = "SELECT * FROM product";

            PrintTableFromSqlDataReader(connection, "product", queryTableProduct);

            PrintToConsole("Введите следующую информацию о товаре:", ConsoleColor.Gray, true, false);
            PrintToConsole("Название товара: ", ConsoleColor.Gray, false, false);
            var productsName = Console.ReadLine();
            PrintToConsole("Стоимость товара: ", ConsoleColor.Gray, false, false);
            _ = decimal.TryParse(Console.ReadLine(), out var productPrice);
            PrintToConsole("Выберите категорию товара из таблицы ниже:", ConsoleColor.Gray, true, false);
            PrintToConsole("", ConsoleColor.White, false, true);
            PrintTableFromSqlDataReader(connection, "productCategories", queryTableProductCategories);
            var productCategory = Console.ReadLine();

            const string queryToWriteNewProduct = "INSERT INTO product(name, price, categoryId) "
                                                  + "SELECT @productsName, @productPrice, productCategories.id "
                                                  + "FROM productCategories "
                                                  + "WHERE productCategories.name = @productCategory";

            RunQuery(queryToWriteNewProduct, connection, command =>
            {
                command.Parameters.Add(new SqlParameter("@productsName", productsName)
                {
                    SqlDbType = SqlDbType.NVarChar
                });

                command.Parameters.Add(new SqlParameter("@productPrice", productPrice)
                {
                    SqlDbType = SqlDbType.Decimal
                });

                command.Parameters.Add(new SqlParameter("@productCategory", productCategory)
                {
                    SqlDbType = SqlDbType.NVarChar
                });

                command.ExecuteNonQuery();
            });

            PrintToConsole("", ConsoleColor.White, false, true);
            PrintTableFromSqlDataReader(connection, "product", queryTableProduct);

            PrintToConsole("3. Отредактировать некоторый товар.", ConsoleColor.Blue, true, true);

            PrintTableFromSqlDataReader(connection, "product", queryTableProduct);
            PrintToConsole("Введите имя товара который желаете отредактировать из таблицы \"Product\" выше: ",
                ConsoleColor.Gray, false, false);
            var productName1 = Console.ReadLine();

            PrintToConsole("Введите новую стоимость товара: ", ConsoleColor.Gray, false, false);
            _ = decimal.TryParse(Console.ReadLine(), out var productPrice1);

            const string queryToChangeProductPrice = "UPDATE product "
                                                     + "SET product.price = @productPrice1 "
                                                     + "WHERE product.name = @productName1";

            RunQuery(queryToChangeProductPrice, connection, command =>
            {
                command.Parameters.Add(new SqlParameter("@productName1", productName1)
                {
                    SqlDbType = SqlDbType.NVarChar
                });

                command.Parameters.Add(new SqlParameter("@productPrice1", productPrice1)
                {
                    SqlDbType = SqlDbType.Decimal
                });

                command.ExecuteNonQuery();
            });

            PrintToConsole("", ConsoleColor.White, false, true);
            PrintTableFromSqlDataReader(connection, "product", queryTableProduct);

            PrintToConsole("4. Удалить некоторый товар.", ConsoleColor.Blue, true, true);
            PrintTableFromSqlDataReader(connection, "product", queryTableProduct);

            PrintToConsole("Выберите имя товара из таблицы \"product\" выше: ", ConsoleColor.Gray, false, false);
            var productName2 = Console.ReadLine();

            const string queryToDeleteProduct = "DELETE FROM product "
                                                + "WHERE product.name = @productName2";

            RunQuery(queryToDeleteProduct, connection, command =>
            {
                command.Parameters.Add(new SqlParameter("@productName2", productName2)
                {
                    SqlDbType = SqlDbType.NVarChar
                });

                command.ExecuteNonQuery();
            });

            PrintToConsole("", ConsoleColor.White, false, true);
            PrintTableFromSqlDataReader(connection, "product", queryTableProduct);

            PrintToConsole("5. Выгрузить весь список товаров вместе с именами категорий через reader, и распечатайте все данные в цикле.",
                ConsoleColor.Blue, true, true);

            const string queryProductsListWithCategories1 = "SELECT product.name Product, productCategories.name Category "
                                                           + "FROM product "
                                                           + "INNER JOIN productCategories "
                                                           + "  ON product.categoryId = productCategories.id";

            PrintTableFromSqlDataReader(connection, "Список товаров с категориями", queryProductsListWithCategories1);

            PrintToConsole("6. Выгрузить весь список товаров вместе с именами категорий в DataSet через SqlDataAdapter, и распечатайте все данные в цикле.",
                ConsoleColor.Blue, true, true);

            const string queryProductsListWithCategories2 = "SELECT product.name Product, productCategories.name Category "
                                                           + "FROM product "
                                                           + "INNER JOIN productCategories "
                                                           + "  ON product.categoryId = productCategories.id";

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
                var adapter = new SqlDataAdapter(query, connection);
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
            var widthAllColumns = maxColumnsWidth.Aggregate((total, column) => total + column);

            PrintToConsole($" {new string('-', widthAllColumns + columnsCount * 3 + 1)}", ConsoleColor.DarkGreen, true, false);

            for (var i = 0; i < columnsCount; i++)
            {
                var (spaces, cellValue) = formHeader(i);

                PrintToConsole($" | {new string(' ', spaces)}{cellValue}", ConsoleColor.DarkGreen, false, false);
            }

            PrintToConsole(" | ", ConsoleColor.DarkGreen, true, false);
            PrintToConsole($" {new string('-', widthAllColumns + columnsCount * 3 + 1)}", ConsoleColor.DarkGreen, true, false);

            var cells = formBody();

            for (var i = 0; i < cells.GetLength(0); i++)
            {
                for (var j = 0; j < cells.GetLength(1); j++)
                {
                    PrintToConsole($" | {new string(' ', cells[i, j].spaces)}{cells[i, j].cellValue}", ConsoleColor.DarkGreen, false, false);
                }

                PrintToConsole(" | ", ConsoleColor.DarkGreen, true, false);
            }

            PrintToConsole($" {new string('-', widthAllColumns + columnsCount * 3 + 1)}", ConsoleColor.DarkGreen, true, false);
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

        private static void PrintToConsole(string? message, ConsoleColor color, bool lineBreakEnabled, bool isAddingEmptyStringEnabled)
        {
            CheckArgument(message);
            CheckArgument(color);

            Console.ForegroundColor = color;

            if (lineBreakEnabled)
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