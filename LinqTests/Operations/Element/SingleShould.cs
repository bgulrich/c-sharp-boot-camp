using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Xunit;

namespace LinqTests.Operations.Element
{
    public class SingleOrDefaultShould
    {
        [Fact]
        public void ReturnTheOnlyItemInTheSequence()
        {
            var items = new[] { 1 };

            var single = items.SingleOrDefault();

            Assert.Equal(1, single);
        }

        [Fact]
        public void ReturnTheOnlyItemInTheSequenceThatMatchesThePredicate()
        {
            var items = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            var single = items.SingleOrDefault(i => i > 2 && i < 4);

            Assert.Equal(3, single);
        }

        [Fact]
        public void ReturnDefaultValueIfSequenceContainsNoElements()
        {
            var items = new int[0];

            var single = items.SingleOrDefault();

            Assert.Equal(default(int), single);
        }

        [Fact]
        public void ThrowInvalidOperationExceptionIfSequenceContainsMoreThanOneElement()
        {
            var items = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            Assert.Throws<InvalidOperationException>(() =>
            {
                var single = items.SingleOrDefault();
            });
        }

        [Fact]
        public void ReturnDefaultValueIfSequenceContainsNoElementsThatMatchThePredicate()
        {
            var items = new []{ 1, 2 };

            var single = items.SingleOrDefault(i => i > 2);

            Assert.Equal(default(int), single);
        }

        [Fact]
        public void ThrowInvalidOperationExceptionIfSequenceContainsMultipleElementsThatMatchThePredicate()
        {
            var items = new[] { 1, 2, 3, 4, 5 };

            Assert.Throws<InvalidOperationException>(() =>
            {
                var single = items.SingleOrDefault(i => i > 2);
            });
        }
    }
}
