using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Xunit;

namespace LinqTests.Operations.Element
{
    public class LastOrDefaultShould
    {
        [Fact]
        public void ReturnTheLastItemInTheSequence()
        {
            var items = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            var last = items.LastOrDefault();

            Assert.Equal(10, last);
        }

        [Fact]
        public void ReturnTheLastItemInTheSequenceThatMatchesThePredicate()
        {
            var items = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            var last = items.LastOrDefault(i => i < 9);

            Assert.Equal(8, last);
        }

        [Fact]
        public void ReturnDefaultValueIfSequenceContainsNoElements()
        {
            var items = new int[0];

            var last = items.LastOrDefault();

            Assert.Equal(default(int), last);
        }

        [Fact]
        public void ReturnDefaultValueIfSequenceContainsNoElementsThatMatchThePredicate()
        {
            var items = new []{ 1, 2 };

            var last = items.LastOrDefault(i => i > 2);

            Assert.Equal(default(int), last);
        }
    }
}
