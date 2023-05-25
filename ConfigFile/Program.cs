using System;
using System.Configuration;

namespace Academits.Karetskas.ConfigFile
{
    internal class Program
    {
        static void Main()
        {
            var siteUrl = ConfigurationManager.AppSettings["SiteUrl"];

            Console.WriteLine($"Site URL: {siteUrl}");
            
            Console.ReadKey();
        }
    }
}
