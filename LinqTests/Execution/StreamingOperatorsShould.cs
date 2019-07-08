using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LinqTests.Model;
using Xunit;

namespace LinqTests.Execution
{
    public class StreamingOperatorsShould
    {

        [Fact]
        public void BeExecutableOnInfiniteSequences()
        {
            var infiniteSequence = new InfiniteSequence();

            var first10LessThanAMillion = infiniteSequence.Where(i => i < 1000000)
                                                          .Take(10)
                                                          .ToArray();

            Assert.Equal(10, first10LessThanAMillion.Length);
        }
    }
}
