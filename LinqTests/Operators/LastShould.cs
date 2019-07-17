using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Xunit;

namespace LinqTests.Operators
{
    public class LastShould
    {
        [Fact]
        public void ReturnTheLastItemInTheSequence()
        {
            var items = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            var last = items.Last();

            Assert.Equal(10, last);
        }

        [Fact]
        public void ReturnTheLastItemInTheSequenceThatMatchesThePredicate()
        {
            var items = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            var last = items.Last(i => i < 9);

            Assert.Equal(8, last);
        }

        [Fact]
        public void ThrowInvalidOperationExceptionIfSequenceContainsNoElements()
        {
            var items = new int[0];

            Assert.Throws<InvalidOperationException>(() =>
            {
                var last = items.Last();
            });
        }

        [Fact]
        public void ThrowInvalidOperationExceptionIfSequenceContainsNoElementsThatMatchThePredicate()
        {
            var items = new []{ 1, 2 };

            Assert.Throws<InvalidOperationException>(() =>
            {
                var last = items.Last(i => i > 2);
            });
        }
    }
}
