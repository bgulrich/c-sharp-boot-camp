using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Xunit;

namespace LinqTests.Operations.Element
{
    public class SingleShould
    {
        [Fact]
        public void ReturnTheOnlyItemInTheSequence()
        {
            var items = new[] { 1 };

            var single = items.Single();

            Assert.Equal(1, single);
        }

        [Fact]
        public void ReturnTheOnlyItemInTheSequenceThatMatchesThePredicate()
        {
            var items = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            var single = items.Single(i => i > 2 && i < 4);

            Assert.Equal(3, single);
        }

        [Fact]
        public void ThrowInvalidOperationExceptionIfSequenceContainsNoElements()
        {
            var items = new int[0];

            Assert.Throws<InvalidOperationException>(() =>
            {
                var single = items.Single();
            });
        }

        [Fact]
        public void ThrowInvalidOperationExceptionIfSequenceContainsMoreThanOneElement()
        {
            var items = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            Assert.Throws<InvalidOperationException>(() =>
            {
                var single = items.Single();
            });
        }

        [Fact]
        public void ThrowInvalidOperationExceptionIfSequenceContainsNoElementsThatMatchThePredicate()
        {
            var items = new []{ 1, 2 };

            Assert.Throws<InvalidOperationException>(() =>
            {
                var single = items.Single(i => i > 2);
            });
        }

        [Fact]
        public void ThrowInvalidOperationExceptionIfSequenceContainsMultipleElementsThatMatchThePredicate()
        {
            var items = new[] { 1, 2, 3, 4, 5 };

            Assert.Throws<InvalidOperationException>(() =>
            {
                var single = items.Single(i => i > 2);
            });
        }
    }
}
