using System;
using Xunit;

namespace TypeTests
{
    public class ClassesShould
    {
        #region Helpers

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

        private class Point : IReadWritePoint
        {
            public float X { get; set;}
            public float Y { get; set; }
        }
        #endregion

        [Fact]
        public void BeAllocatedOnTheHeapWhenDeclaredAsLocals()
        {
            var initialMemoryUsed = GC.GetTotalMemory(true);

            var a = new Point();
            var b = new Point();
            var c = new Point();

            var increasedMemoryFootprint = GC.GetTotalMemory(true) - initialMemoryUsed;

            // expected minimum size increase = (2 x float) x count
            var minSizeIncrease = 2 * sizeof(float) * 3;

            Assert.True(increasedMemoryFootprint > minSizeIncrease);
        }
    }
}
