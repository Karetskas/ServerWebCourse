using System;
using System.Collections.Generic;

namespace NumbersDivision
{
    internal class Program
    {
        static void Main(string[] args)
        {
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

            const double divisibleNumber = 5;

            numbers.ForEach(divider =>
            {
                double result;

                try
                {
                    result = Division(divisibleNumber, divider);
                }
                catch(DivideByZeroException e)
                {
                    Console.WriteLine($"Error => {e}");

                    return;
                }

                Console.WriteLine($"{divisibleNumber} / {divider} = {result}");
            });

            Console.ReadKey();
        }

        private static double Division(double number1, double number2)
        {
            const double epsilon = 1.0e-10;

            if (number2 >= -epsilon && number2 <= epsilon)
            {
                throw new DivideByZeroException($"The argument \"{nameof(number2)}\" = {nameof(number2)} isn't must be equal to 0.");
            }

            return number1 / number2;
        }
    }
}
