using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace TypeTests
{
    public class BoxingAndUnboxingShould
    {
        #region Types
        private struct Point
        {
            public double X { get; set; }
            public double Y { get; set; }

            public Point(double x, double y)
            {
                X = x;
                Y = y;
            }
        }
        #endregion

        [Fact]
        public void ReducePerformance()
        {
            int iterations = 10000000;

            var start = DateTime.Now;

            for(float i = 0; i < iterations; ++i)
            {
                var p = new Point(i, i);
            }

            var noBoxingTime = DateTime.Now - start;

            start = DateTime.Now;

            for (float i = 0; i < iterations; ++i)
            {
                // explicit cast
                // var o = (object)new Point(i, i);
                // implicit
                object o = new Point(i, i);
                var p = (Point)o;
            }

            var boxingUnboxingTime = DateTime.Now - start;

            // > 20% performance hit
            Assert.True(boxingUnboxingTime > noBoxingTime * 1.2);
        }

    }
}
