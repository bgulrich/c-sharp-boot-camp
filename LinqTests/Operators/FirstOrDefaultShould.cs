using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Xunit;

namespace LinqTests.Operators
{
    public class FirstOrDefaultShould
    {
        [Fact]
        public void ReturnTheFirstItemInTheSequence()
        {
            var items = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            var first = items.FirstOrDefault();

            Assert.Equal(1, first);
        }

        [Fact]
        public void ReturnTheFirstItemInTheSequenceThatMatchesThePredicate()
        {
            var items = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            var first = items.FirstOrDefault(i => i > 2);

            Assert.Equal(3, first);
        }

        [Fact]
        public void ReturnDefaultValueIfSequenceContainsNoElements()
        {
            var items = new int[0];

            var first = items.FirstOrDefault();

            Assert.Equal(default(int), first);
        }

        [Fact]
        public void ReturnDefaultValueIfSequenceContainsNoElementsThatMatchThePredicate()
        {
            var items = new []{ 1, 2 };

            var first = items.FirstOrDefault(i => i > 2);

            Assert.Equal(default(int), first);
        }
    }
}
