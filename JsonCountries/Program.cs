using System;
using System.IO;
using System.Text;
using System.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Academits.Karetskas.JsonCountries
{
    public class Program
    {
        public static void Main()
        {
            if (!File.Exists(@"countries.json"))
            {
                return;
            }

            var jsonText = File.ReadAllText(@"Countries.json");

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

            var totalPopulationCountries = countries.Aggregate(0L, (total, country) => total + country.Population);
            Console.WriteLine($"Total population by countries = {totalPopulationCountries}");
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