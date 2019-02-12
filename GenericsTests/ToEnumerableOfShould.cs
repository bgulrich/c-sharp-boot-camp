using Generics;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace GenericsTests
{
    public class ToEnumerableOfShould
    {
        [Fact]
        public void DoubleToInt()
        {
            var doubles = new List<double> { 1.5, 2.0, 3.7, 5.9, 6.3 };
            // converter rounds result
            var expected = new List<int> { 2, 2, 4, 6, 6 };

            int i = 0;

            foreach(var integer in doubles.ToEnumerableOf<double, int>())
            {
                Assert.Equal(expected[i], integer);
                ++i;
            }           
        }      
    }
}
