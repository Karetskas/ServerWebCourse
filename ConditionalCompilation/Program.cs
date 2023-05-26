using System;

namespace Academits.Karetskas.ConditionalCompilation
{
    internal class Program
    {
        static void Main()
        {
            #region Conditional Compilation
#if DEBUG
            Console.WriteLine("Выполнение кода в Debug версии.");
#else
            Console.WriteLine("Выполнение кода в остальных версиях.");
#endif
            #endregion
        }
    }
}