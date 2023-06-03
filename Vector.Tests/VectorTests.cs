using Xunit;
using System;

using VectorOop = Academits.Karetskas.VectorTask.Vector;

namespace Academits.Karetskas.Vector.Tests
{
    public class VectorTests
    {
        [Theory]
        [InlineData(null)]
        public void Add_InputNull_OutputError(VectorOop vectorToAdd)
        {
            const int size = 3;

            var currentVector = new VectorOop(size);

            Assert.Throws<ArgumentNullException>(() => currentVector.Add(vectorToAdd));
        }

        [Fact]
        public void Add_ToAddWithSameComponents_OutputVector012()
        {
            var vector = new VectorOop(new double[] { 1, 1, 1 });
            var vectorToAdd = new VectorOop(new double[] { -1, 0, 1 });
            var expectedVector = new VectorOop(new double[] { 0, 1, 2 });

            vector.Add(vectorToAdd);

            Assert.Equal(expectedVector, vector);
        }

        [Fact]
        public void Add_ToAddWithManyComponents_OutputVector001()
        {
            var vector = new VectorOop(new double[] { 1 });
            var vectorToAdd = new VectorOop(new double[] { -1, 0, 1 });
            var expectedVector = new VectorOop(new double[] { 0, 0, 1 });

            vector.Add(vectorToAdd);

            Assert.Equal(expectedVector, vector);
        }

        [Fact]
        public void Add_ToAddWithFewerComponents_OutputVector011()
        {
            var vector = new VectorOop(new double[] { 1, 1, 1 });
            var vectorToAdd = new VectorOop(new double[] { -1, 0 });
            var expectedVector = new VectorOop(new double[] { 0, 1, 1 });

            vector.Add(vectorToAdd);

            Assert.Equal(expectedVector, vector);
        }

        [Fact]
        public void GetScalarProduct_InputNullAndVector_OutputError()
        {
            var vector2 = new VectorOop(new double[] { -1, 0, 1 });

            Assert.Throws<ArgumentNullException>(() => VectorOop.GetScalarProduct(null!, vector2));
        }

        [Fact]
        public void GetScalarProduct_InputVectorAndNull_OutputError()
        {
            var vector1 = new VectorOop(new double[] { -1, 0, 1 });

            Assert.Throws<ArgumentNullException>(() => VectorOop.GetScalarProduct(vector1, null!));
        }

        [Fact]
        public void GetScalarProduct_ToMultiplySmallerComponentsByLargerOne_Output7()
        {
            var vector1 = new VectorOop(new double[] { 3, 2 });
            var vector2 = new VectorOop(new double[] { 1, 2, 3 });

            var result = VectorOop.GetScalarProduct(vector1, vector2);

            Assert.Equal(7, result);
        }

        [Fact]
        public void GetScalarProduct_ToMultiplyLargerComponentsBySmallerOne_Output7()
        {
            var vector1 = new VectorOop(new double[] { 1, 2, 3 });
            var vector2 = new VectorOop(new double[] { 3, 2 });

            var result = VectorOop.GetScalarProduct(vector1, vector2);

            Assert.Equal(7, result);
        }

        [Fact]
        public void GetScalarProduct_ToMultiplyEqualInComponentsCount_Output4()
        {
            var vector1 = new VectorOop(new double[] { -1, 2, 3 });
            var vector2 = new VectorOop(new double[] { 3, 2, 1 });

            var result = VectorOop.GetScalarProduct(vector1, vector2);

            Assert.Equal(4, result);
        }
    }
}