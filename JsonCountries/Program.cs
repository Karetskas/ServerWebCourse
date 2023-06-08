using System;
using System.IO;
using System.Text;
using System.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;
using Academits.Karetskas.JsonCountries.JsonClasses;

namespace Academits.Karetskas.JsonCountries
{
    public class Program
    {
        public static void Main()
        {
            const string fileName = "countries.json";

            if (!File.Exists(fileName))
            {
                return;
            }

            var jsonText = File.ReadAllText(fileName);

            if (string.IsNullOrEmpty(jsonText))
            {
                return;
            }

            var countries = JsonConvert.DeserializeObject<List<Country>>(jsonText);

            if (countries is null)
            {
                return;
            }

            Console.OutputEncoding = Encoding.UTF8;

            var countriesTotalPopulation = countries.Aggregate(0L, (total, country) => total + country.Population);
            Console.WriteLine($"Total population by countries = {countriesTotalPopulation}");
            Console.WriteLine();

            var currencies = countries
                .SelectMany(country => country.Currencies?.Select(currency => currency.Code) ?? Enumerable.Empty<string>())
                .Distinct()
                .OrderBy(currencyCode => currencyCode)
                .ToArray();

            Console.WriteLine("List of currencies:");

            for (var i = 0; i < currencies.Length; i++)
            {
                Console.WriteLine($"   {i + 1}. {currencies[i]}");
            }
        }
    }
}