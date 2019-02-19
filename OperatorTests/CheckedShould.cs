using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace OperatorTests
{
    public class CheckedShould
    {
        [Fact]
        public void ThrowAnOverflowExceptionIfWrappedOperationCausesOverflow()
        {
            var someInt = int.MaxValue;

            Assert.Throws<OverflowException>(() => checked(someInt += 1));

            // value doesn't move
            Assert.Equal(int.MaxValue, someInt);
        }
    }
}
