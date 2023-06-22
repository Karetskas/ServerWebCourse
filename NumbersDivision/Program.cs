using System;
using System.Collections.Generic;
using NLog;

namespace Academits.Karetskas.NumbersDivision
{
    internal class Program
    {
        static void Main()
        {
            var logger = LogManager.GetLogger("MyFirstLogger");

            logger.Trace("Запуск программы.");

            var numbers = new List<double>
            {
                0.43,
                54.85454,
                -31.323335,
                9.4333267803,
                -44.96509,
                983.69,
                1.0e-11,
                95.5,
                5,
                10,
                -1.0e-10,
                -0.008338
            };

            logger.Debug($"Список делителей: {string.Join(", ", numbers)}");
            logger.Info("Создан набор делителей для деления чисел");

            const double divisibleNumber = 5;

            numbers.ForEach(divider =>
            {
                try
                {
                    double result = Division(divisibleNumber, divider);

                    logger.Debug("Деление чисел: {divisibleNumber} / {divider} = {result}", divisibleNumber, divider, result);
                }
                catch (DivideByZeroException e)
                {
                    logger.Error(e, "Попытка деления на ноль.");
                }
            });

            logger.Trace("Программа закончила свою работу.");

            Console.ReadKey();
        }

        private static double Division(double number1, double number2)
        {
            const double epsilon = 1.0e-10;

            if (Math.Abs(number2) <= epsilon)
            {
                throw new DivideByZeroException($"The argument \"{nameof(number2)}\" = {nameof(number2)} isn't must be equal to 0.");
            }

            return number1 / number2;
        }
    }
}
