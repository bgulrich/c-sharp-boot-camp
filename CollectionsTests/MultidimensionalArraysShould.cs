using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace CollectionTests
{
    public class MultidimensionalArraysShould
    {
        [Fact]
        public void ReportTheLengthAsDimensionsMultiplied()
        {
            var lengthX = 100;
            var lengthY = 200;
            var lengthZ = 50;

            Assert.Equal(lengthX * lengthY * lengthZ, new int[lengthX, lengthY, lengthZ].Length);
        }

        [Fact]
        public void ReportTheLengthOfDimensionsUsingGetLength()
        {
            var lengthX = 100;
            var lengthY = 200;
            var lengthZ = 50;

            var array = new int[lengthX, lengthY, lengthZ];

            Assert.Equal(lengthX, array.GetLength(0));
            Assert.Equal(lengthY, array.GetLength(1));
            Assert.Equal(lengthZ, array.GetLength(2));
        }

        [Fact]
        public void ReportTheNumberOfDimensionsWithTheRankProperty()
        {
            var lengthX = 100;
            var lengthY = 200;
            var lengthZ = 50;

            Assert.Equal(3, new int[lengthX, lengthY, lengthZ].Rank);
        }

        [Fact]
        public void ReturnElementsInZeroBasedIndexOrderPerDimension()
        {
            // 4 x 3 array
            var array = new int[,]
            {
                { 0, 1, 2 },
                { 1, 2, 3},
                { 2, 3, 4 },
                { 3, 4, 5 }
            };

            for (var x = 0; x < array.GetLength(0); ++x)
            {
                for (var y = 0; y < array.GetLength(1); ++y)
                {
                    Assert.Equal(y + x, array[x, y]);
                }
            }
        }
    }
}
