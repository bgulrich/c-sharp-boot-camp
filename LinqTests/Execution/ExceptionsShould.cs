using System;
using System.Linq;
using Xunit;

namespace LinqTests.Execution
{
    public class ExceptionsShould
    {
        private readonly int[] _numbers = new[] {1, 2, 5, 8, 9, 42, 684, 455};


        [Fact]
        public void BeThrownImmediatelyForNonDeferredQueries()
        {
            int badQueryResult;

            Assert.Throws<DivideByZeroException>(() => badQueryResult  =_numbers.Count(i => i / 0 > 1));
        }

        [Fact]
        public void BeDeferredForDeferredQueries()
        {
            int badQueryResult;
            var badQuery = _numbers.Where(i => i / 0 > 1);

            Assert.Throws<DivideByZeroException>(() => { badQueryResult = badQuery.Count(); });
        }
    }
}
