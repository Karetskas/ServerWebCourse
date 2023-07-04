using System;
using System.Linq;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;

namespace Academits.Karetskas.Transactions
{
    internal class Program
    {
        static void Main()
        {
            const string connectionString = "Server=Micron;Database=Shop;Encrypt=True;TrustServerCertificate=True;Trusted_Connection=True;";
            using var connection = new SqlConnection(connectionString);
            connection.Open();

            PrintToConsole($"\"Connection state = {connection.State}", ConsoleColor.Yellow, true, true);

            PrintToConsole($"1. Подключится из кода к БД и начать транзакцию.{Environment.NewLine}"
                           + $"- Вставить категорию.{Environment.NewLine}"
                           + $"- Кинуть исключение и отменить транзакцию.{Environment.NewLine}"
                           + "- Убедится что категория не добавилась.", ConsoleColor.Blue, true, true);

            PrintToConsole("Таблица категорий до изменения:", ConsoleColor.Gray, true, true);

            const string queryTableProductCategories1 = "SELECT * FROM ProductCategory";

            PrintTableFromSqlDataReader(connection, "Product categories", queryTableProductCategories1);

            const string queryToAddProductCategory1 = "INSERT INTO ProductCategory(Name) "
                                                     + "VALUES (N'Sweets')";

            MakeTransaction(connection, queryToAddProductCategory1, () => throw new Exception("Error occurred during the transaction!"));

            PrintToConsole("Таблица категорий после изменения:", ConsoleColor.Gray, true, true);

            PrintTableFromSqlDataReader(connection, "Product categories", queryTableProductCategories1);

            PrintToConsole($"2. Подключится из кода к БД.{Environment.NewLine}"
                           + $"- Вставить категорию.{Environment.NewLine}"
                           + $"- Кинуть исключение.{Environment.NewLine}"
                           + "- Убедится что категория добавилась.", ConsoleColor.Blue, true, true);

            PrintToConsole("Таблица категорий до изменения:", ConsoleColor.DarkYellow, true, true);

            const string queryTableProductCategories2 = "SELECT * FROM ProductCategory";

            PrintTableFromSqlDataReader(connection, "Product categories", queryTableProductCategories2);

            const string queryToAddProductCategory2 = "INSERT INTO ProductCategory(Name) "
                                                     + "VALUES (N'Sweets')";

            try
            {
                using var command = new SqlCommand(queryToAddProductCategory2, connection);
                command.ExecuteNonQuery();

                throw new Exception("Error occurred during the transaction!");
            }
            catch (Exception e)
            {
                PrintToConsole(e.ToString(), ConsoleColor.Red, true, true);
            }

            PrintToConsole("Таблица категорий после изменения:", ConsoleColor.DarkYellow, true, true);

            PrintTableFromSqlDataReader(connection, "Product categories", queryTableProductCategories1);
        }

        private static void MakeTransaction(SqlConnection? connection, string query, Action task)
        {
            CheckArgument(connection);
            CheckArgument(query);

            using var transaction = connection!.BeginTransaction();

            try
            {
                using var command = new SqlCommand(query, connection);
                command.Transaction = transaction;
                command.ExecuteNonQuery();

                task();

                transaction.Commit();
            }
            catch (Exception e)
            {
                PrintToConsole(e.ToString(), ConsoleColor.Red, true, true);

                transaction.Rollback();
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
                PrintToConsole(e.ToString(), ConsoleColor.Red, true, true);
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