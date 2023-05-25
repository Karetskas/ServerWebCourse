using System;

namespace Academits.Karetskas.ReSharper
{
    internal class Program
    {
        static void Main()
        {
            Console.WriteLine("Merge sort.");
            Console.WriteLine();
            
            int[] array = { 13, 3, 8, 1, 15, 2, 3, 7, 4 };

            Console.WriteLine($"Array before sort: {string.Join(" ", array)}");
            Console.WriteLine();

            for (var i = 1; i <= array.Length; i *= 2)
            {
                var firstPointer = 0;
                var secondPointer = i;
                var resultArray = new int[array.Length];
                var resultArrayPointer = 0;

                while (firstPointer < array.Length)
                {
                    var firstPointerLimit = secondPointer < array.Length
                        ? secondPointer
                        : array.Length;

                    var secondPointerLimit = firstPointerLimit + i < array.Length
                        ? firstPointerLimit + i
                        : array.Length;

                    while(firstPointer < firstPointerLimit || secondPointer < secondPointerLimit)
                    {
                        var isFirstPointerLimit = (firstPointer >= array.Length && secondPointer < array.Length) || firstPointer == firstPointerLimit;
                        var isSecondPointerLimit = (secondPointer >= array.Length && firstPointer < array.Length) || secondPointer == secondPointerLimit;
                        var isFirstItemGreater = firstPointer < array.Length && secondPointer < array.Length && array[firstPointer] > array[secondPointer];

                        if (isFirstPointerLimit || (!isSecondPointerLimit && isFirstItemGreater))
                        {
                            resultArray[resultArrayPointer] = array[secondPointer];
                            resultArrayPointer++;
                            secondPointer++;

                            continue;
                        }

                        resultArray[resultArrayPointer] = array[firstPointer];
                        resultArrayPointer++;
                        firstPointer++;
                    }
                    
                    firstPointer = secondPointer;
                    secondPointer = firstPointer + i;
                }

                array = resultArray;
            }

            Console.WriteLine();
            Console.WriteLine($"Array after sort: {string.Join(" ", array)}");
        }
    }
}