using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LinqTests.Model;
using Xunit;

namespace LinqTests.Execution
{
    public class ImmediateExecutionOperatorsShould
    {
        [Fact]
        public void FailWhenExecutedOnAnInfiniteSequence()
        {
            var infiniteSequence = new InfiniteSequence();

            Assert.Throws<OverflowException>(() =>
            {
                var sum = infiniteSequence.Sum();
            });
        }
    }
}
