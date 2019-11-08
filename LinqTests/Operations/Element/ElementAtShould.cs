using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace LinqTests.Operations.Element
{
    public class ElementAtShould
    {
        [Fact]
        public void ReturnTheElementAtTheSpecifiedIndex()
        {
            var ints = new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            for(int i = 0; i < ints.Length; ++i)
            {
                Assert.Equal(i, ints.ElementAt(i));
            }
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(10)]
        public void ThrowAnArgumentOutOfRangeExceptionIfTheSpecifiedIndexIsOutOfRange(int index)
        {
            var ints = new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            Assert.Throws<ArgumentOutOfRangeException>(() => { var i = ints.ElementAt(index); });
        }
    }
}
