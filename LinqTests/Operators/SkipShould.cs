using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Xunit;

namespace LinqTests.Operators
{
    public class SkipShould
    {
        [Fact]
        public void SkipCountItemsInTheSequenceIfTheSequenceLengthIsAtLeastCountItems()
        {
            var items = new[] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10};

            var last5 = items.Skip(5);

            Assert.Equal(5, last5.Count());

            var expected = 6;

            foreach (var i in last5)
            {
                Assert.Equal(expected++, i);
            }
        }

        [Fact]
        public void ReturnEmptySequenceIfSequenceContainsCountItemsOrLess()
        {
            var items = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            var empty = items.Skip(11);

            Assert.Empty(empty);
        }
    }
}
