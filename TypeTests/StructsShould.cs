using System;
using Xunit;

namespace TypeTests
{
    public class StructsShould
    {
        #region Read-only

        private interface IReadOnlyPoint
        {
            float X { get; }
            float Y { get; }
        }

        private interface IWriteOnlyPoint
        {
            float X { set; }
            float Y { set; }
        }

        private interface IReadWritePoint : IReadOnlyPoint, IWriteOnlyPoint
        { }

        private readonly struct Point_ReadOnly : IReadOnlyPoint // illegal -> , IWriteOnlyPoint, IReadWritePoint
        {
            public float X { get; } // illegal -> set; }
            public float Y { get; }

            public Point_ReadOnly(float x, float y)
            {
                X = x;
                Y = y;
            }

            public void SetY(float y)
            {
                // illegal
                // Y = y;
            }
        }
        #endregion

        [Fact]
        public void BeImplementableAsReadOnly()
        {
            var p = new Point_ReadOnly(5, 5);

            // illegal
            // p.X = 10;
        }

        [Fact]
        public void BeAllocatedOnTheStackWhenDeclaredAsLocals()
        {
            var initialMemoryUsed = GC.GetTotalMemory(true);

            for (int i = 0; i < 1000000; ++i)
            {
                new Point_ReadOnly();
            }

            Assert.Equal(initialMemoryUsed, GC.GetTotalMemory(true));
        }

        [Fact]
        public void BeBoxedToTheHeapWhenAddedToAReferenceType()
        {
            var initialMemoryUsed = GC.GetTotalMemory(true);

            var array = new Point_ReadOnly[1000000];

            for (int i = 0; i < 1000000; ++i)
            {
                array[i] = new Point_ReadOnly();
            }

            var increasedMemoryFootprint = GC.GetTotalMemory(true) - initialMemoryUsed;

            // expected minimum size increase = (2 x float) x count
            var minSizeIncrease = 2 * sizeof(float) * array.Length;

            Assert.True(increasedMemoryFootprint > minSizeIncrease);
        }

        private void CreateTriangle(in Point_ReadOnly a, in Point_ReadOnly b, in Point_ReadOnly c)
        {

        }

        [Fact]
        public void BeBoxedToTheHeapWhenPassedByReference()
        {
            var initialMemoryUsed = GC.GetTotalMemory(true);

            var a = new Point_ReadOnly();
            var b = new Point_ReadOnly();
            var c = new Point_ReadOnly();

            Assert.Equal(initialMemoryUsed, GC.GetTotalMemory(true));

            CreateTriangle(a, b, c);

            // expected minimum size increase = (2 x float) x count
            var minSizeIncrease = 2 * sizeof(float) * 3;

            var increasedMemoryFootprint = GC.GetTotalMemory(true) - initialMemoryUsed;
            Assert.True(increasedMemoryFootprint > minSizeIncrease);
        }
    }
}
