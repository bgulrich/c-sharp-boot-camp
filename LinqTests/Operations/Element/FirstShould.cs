using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Xunit;

namespace LinqTests.Operations.Element
{
    public class FirstShould
    {
        [Fact]
        public void ReturnTheFirstItemInTheSequence()
        {
            var items = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            var first = items.First();

            Assert.Equal(1, first);
        }

        [Fact]
        public void ReturnTheFirstItemInTheSequenceThatMatchesThePredicate()
        {
            var items = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            var first = items.First(i => i > 2);

            Assert.Equal(3, first);
        }

        [Fact]
        public void ThrowInvalidOperationExceptionIfSequenceContainsNoElements()
        {
            var items = new int[0];

            Assert.Throws<InvalidOperationException>(() =>
            {
                var first = items.First();
            });
        }

        [Fact]
        public void ThrowInvalidOperationExceptionIfSequenceContainsNoElementsThatMatchThePredicate()
        {
            var items = new []{ 1, 2 };

            Assert.Throws<InvalidOperationException>(() =>
            {
                var first = items.First(i => i > 2);
            });
        }
    }
}
