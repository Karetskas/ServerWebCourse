using System;

namespace Academits.Karetskas.VectorTask
{
    public sealed class Vector
    {
        private double[] components;

        public int Size => components.Length;

        public double Length
        {
            get
            {
                double componentsSquaredSum = 0;

                foreach (double component in components)
                {
                    componentsSquaredSum += component * component;
                }

                return Math.Sqrt(componentsSquaredSum);
            }
        }

        public double this[int index]
        {
            get
            {
                if (index < 0 || index >= components.Length)
                {
                    throw new ArgumentOutOfRangeException(nameof(index), $"The \"{nameof(index)}\" = {index} argument is out of range of array. " +
                        $"Valid range is from 0 to {components.Length - 1}.");
                }

                return components[index];
            }

            set
            {
                if (index < 0 || index >= components.Length)
                {
                    throw new ArgumentOutOfRangeException(nameof(index), $"The \"{nameof(index)}\" = {index} argument is out of range of array. " +
                        $"Valid range is from 0 to {components.Length - 1}.");
                }

                components[index] = value;
            }
        }

        public Vector(int size)
        {
            if (size <= 0)
            {
                throw new ArgumentException($"Argument \"{nameof(size)}\" <= 0.", nameof(size));
            }

            components = new double[size];
        }

        public Vector(double[] components)
        {
            if (components is null)
            {
                throw new ArgumentNullException($"{nameof(components)}", $"Argument of \"{nameof(components)}\" is null.");
            }

            if (components.Length == 0)
            {
                throw new ArgumentException($"Argument \"{nameof(components)}.{nameof(components.Length)}\" is 0.");
            }

            this.components = new double[components.Length];

            components.CopyTo(this.components, 0);
        }

        public Vector(int size, double[] components)
        {
            if (components is null)
            {
                throw new ArgumentNullException(nameof(components), $"Argument of \"{nameof(components)}\" is null.");
            }

            if (size <= 0)
            {
                throw new ArgumentException($"Argument \"{nameof(size)}\" <= 0.", nameof(size));
            }

            this.components = new double[size];

            Array.Copy(components, this.components, size);
        }

        public Vector(Vector vector)
        {
            if (vector is null)
            {
                throw new ArgumentNullException(nameof(vector), $"Argument \"{nameof(vector)}\" is null.");
            }

            components = new double[vector.Size];

            vector.components.CopyTo(components, 0);
        }

        public void Add(Vector vector)
        {
            if (vector is null)
            {
                throw new ArgumentNullException(nameof(vector), $"Argument \"{nameof(vector)}\" is null.");
            }

            if (components.Length < vector.Size)
            {
                Array.Resize(ref components, vector.Size);
            }

            for (int i = 0; i < vector.Size; i++)
            {
                components[i] += vector[i];
            }
        }

        public void Subtract(Vector vector)
        {
            if (vector is null)
            {
                throw new ArgumentNullException(nameof(vector), $"Argument \"{nameof(vector)}\" is null.");
            }

            if (components.Length < vector.Size)
            {
                Array.Resize(ref components, vector.Size);
            }

            for (int i = 0; i < vector.Size; i++)
            {
                components[i] -= vector[i];
            }
        }

        public void MultiplyByScalar(double number)
        {
            for (int i = 0; i < components.Length; i++)
            {
                components[i] *= number;
            }
        }

        public void Reverse()
        {
            MultiplyByScalar(-1);
        }

        public static Vector GetSum(Vector vector1, Vector vector2)
        {
            return vector1 + vector2;
        }

        public static Vector GetDifference(Vector vector1, Vector vector2)
        {
            return vector1 - vector2;
        }

        public static double GetScalarProduct(Vector vector1, Vector vector2)
        {
            if (vector1 is null)
            {
                throw new ArgumentNullException(nameof(vector1), $"Argument \"{nameof(vector1)}\" is null.");
            }

            if (vector2 is null)
            {
                throw new ArgumentNullException(nameof(vector2), $"Argument \"{nameof(vector2)}\" is null.");
            }

            int minVectorSize = Math.Min(vector1.Size, vector2.Size);
            double scalarProduct = 0;

            for (int i = 0; i < minVectorSize; i++)
            {
                scalarProduct += vector1[i] * vector2[i];
            }

            return scalarProduct;
        }

        public static Vector operator +(Vector vector1, Vector vector2)
        {
            if (vector1 is null)
            {
                throw new ArgumentNullException(nameof(vector1), $"Argument \"{nameof(vector1)}\" is null.");
            }

            if (vector2 is null)
            {
                throw new ArgumentNullException(nameof(vector2), $"Argument \"{nameof(vector2)}\" is null.");
            }

            Vector vectorsSum = new Vector(vector1);

            vectorsSum.Add(vector2);

            return vectorsSum;
        }

        public static Vector operator -(Vector vector1, Vector vector2)
        {
            if (vector1 is null)
            {
                throw new ArgumentNullException(nameof(vector1), $"Argument \"{nameof(vector1)}\" is null.");
            }

            if (vector2 is null)
            {
                throw new ArgumentNullException(nameof(vector2), $"Argument \"{nameof(vector2)}\" is null.");
            }

            Vector vectorsDifference = new Vector(vector1);

            vectorsDifference.Subtract(vector2);

            return vectorsDifference;
        }

        public static Vector operator *(Vector vector, double number)
        {
            if (vector is null)
            {
                throw new ArgumentNullException(nameof(vector), $"Argument \"{nameof(vector)}\" is null.");
            }

            Vector newVector = new Vector(vector);

            newVector.MultiplyByScalar(number);

            return newVector;
        }

        public static Vector operator *(double number, Vector vector)
        {
            if (vector is null)
            {
                throw new ArgumentNullException(nameof(vector), $"Argument \"{nameof(vector)}\" is null.");
            }

            Vector newVector = new Vector(vector);

            newVector.MultiplyByScalar(number);

            return newVector;
        }

        public override string ToString()
        {
            return $"{{{string.Join(", ", components)}}}";
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(obj, this))
            {
                return true;
            }

            if (obj is null || GetType() != obj.GetType())
            {
                return false;
            }

            Vector vector = (Vector)obj;

            if (components.Length != vector.components.Length)
            {
                return false;
            }

            for (int i = 0; i < components.Length; i++)
            {
                if (components[i] != vector[i])
                {
                    return false;
                }
            }

            return true;
        }

        public override int GetHashCode()
        {
            int prime = 53;
            int hash = 1;

            hash = prime * hash + components.Length;

            foreach (double component in components)
            {
                hash = prime * hash + component.GetHashCode();
            }

            return hash;
        }
    }
}