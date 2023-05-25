using System;
using Academits.Karetskas.TaskToSignAssembly.Library;

namespace Academits.Karetskas.TaskToSignAssembly.MyProgram
{
    internal class Program
    {
        static void Main()
        {
            MyLibrary.PrintToConsole("Calling the \"PrintToConsole\" command from the library.");

            Console.ReadKey();
        }
    }
}
