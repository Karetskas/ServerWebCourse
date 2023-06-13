using System;

namespace Academits.Karetskas.VectorTask
{
    internal class Program
    {
        static void Main()
        {
            double[] components1 = { -12, 22, -32 };
            double[] components2 = { -42, -24, -42, -44, -54 };
            double[] components3 = { 1, 2, 3, 4 };

            Vector mainVector = new Vector(components3);

            Vector[] vectors =
            {
                new Vector(5),
                new Vector(components1),
                new Vector(5, components2),
                new Vector(mainVector)
            };

            string[] stringsArray = new string[vectors.Length];
            Vector additionResult = new Vector(mainVector);

            for (int i = 0; i < vectors.Length; i++)
            {
                additionResult.Add(vectors[i]);

                stringsArray[i] = $"{mainVector} + {vectors[i],27} = {additionResult}";

                additionResult = new Vector(mainVector);
            }

            string title = "Additing of one vector from another:";
            PrintToConsole(ConsoleColor.Red, title, stringsArray);

            Vector subtractionResult = new Vector(mainVector);

            for (int i = 0; i < vectors.Length; i++)
            {
                subtractionResult.Subtract(vectors[i]);

                stringsArray[i] = $"{mainVector} - {vectors[i],27} = {subtractionResult}";

                subtractionResult = new Vector(mainVector);
            }

            title = "Subtraction of one vector from another:";
            PrintToConsole(ConsoleColor.Yellow, title, stringsArray);

            const double number = 2.1;

            for (int i = 0; i < vectors.Length; i++)
            {
                Vector multiplyingVectorByScalarResult = new Vector(vectors[i]);

                multiplyingVectorByScalarResult.MultiplyByScalar(number);

                stringsArray[i] = $"{vectors[i],27} * {number} = {multiplyingVectorByScalarResult}";
            }

            title = "Multiply a vector by a scalar:";
            PrintToConsole(ConsoleColor.Green, title, stringsArray);

            for (int i = 0; i < vectors.Length; i++)
            {
                Vector reversalResult = new Vector(vectors[i]);

                reversalResult.Reverse();

                stringsArray[i] = $"{vectors[i],27} * -1 = {reversalResult}";
            }

            title = "Vector reversal:";
            PrintToConsole(ConsoleColor.Magenta, title, stringsArray);

            for (int i = 0; i < vectors.Length; i++)
            {
                stringsArray[i] = $"{vectors[i],27} = {vectors[i].Length}";
            }

            title = "Get length of the vector:";
            PrintToConsole(ConsoleColor.Blue, title, stringsArray);

            stringsArray = new string[mainVector.Size];
            Vector vectorToSetComponents = new Vector(mainVector);

            for (int i = 0; i < vectorToSetComponents.Size; i++)
            {
                vectorToSetComponents[i] = 3;

                stringsArray[i] = $"The index [{i}] in the {vectorToSetComponents} vector has been changed.";
            }

            title = "Set number \"3\" for the components of the vector:";
            PrintToConsole(ConsoleColor.Cyan, title, stringsArray);

            Vector vectorToGetComponents = new Vector(mainVector);

            for (int i = 0; i < vectorToGetComponents.Size; i++)
            {
                stringsArray[i] = $"In the {vectorToGetComponents} vector: index [{i}] = {vectorToGetComponents[i]}";
            }

            title = "Get vector components by index:";
            PrintToConsole(ConsoleColor.DarkBlue, title, stringsArray);

            stringsArray = new string[vectors.Length];

            for (int i = 0; i < vectors.Length; i++)
            {
                stringsArray[i] = $"{mainVector} and {vectors[i],27} = {mainVector.Equals(vectors[i])}";
            }

            title = "Vectors comparison:";
            PrintToConsole(ConsoleColor.DarkCyan, title, stringsArray);

            for (int i = 0; i < vectors.Length; i++)
            {
                stringsArray[i] = $"Hash code of {vectors[i],27} vector = {vectors[i].GetHashCode()}";
            }

            title = "Get hash code of vectors:";
            PrintToConsole(ConsoleColor.DarkGreen, title, stringsArray);

            for (int i = 0; i < vectors.Length; i++)
            {
                stringsArray[i] = $"{mainVector} + {vectors[i],27} = {Vector.GetSum(mainVector, vectors[i])}";
            }

            title = "Addition of two vectors:";
            PrintToConsole(ConsoleColor.DarkMagenta, title, stringsArray);

            for (int i = 0; i < vectors.Length; i++)
            {
                stringsArray[i] = $"{mainVector} - {vectors[i],27} = {Vector.GetDifference(mainVector, vectors[i])}";
            }

            title = "Subtraction of two vectors:";
            PrintToConsole(ConsoleColor.DarkRed, title, stringsArray);

            for (int i = 0; i < vectors.Length; i++)
            {
                stringsArray[i] = $"{mainVector} * {vectors[i],27} = {Vector.GetScalarProduct(mainVector, vectors[i])}";
            }

            title = "Scalar product of vectors:";
            PrintToConsole(ConsoleColor.DarkYellow, title, stringsArray);
        }

        private static void PrintToConsole(ConsoleColor color, string title, string[] stringsArray)
        {
            Console.WriteLine(new string('-', 90));
            Console.ForegroundColor = color;
            Console.WriteLine(title);

            foreach (string line in stringsArray)
            {
                Console.WriteLine(line);
            }

            Console.ResetColor();
        }
    }
}