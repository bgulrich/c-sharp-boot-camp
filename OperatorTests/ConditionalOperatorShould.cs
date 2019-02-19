using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace OperatorTests
{
    public class ConditionalOperatorShould
    {
        [Fact]
        public void ReturnLeftValueIfExpressionIsTrue()
        {
            DateTime? dt = DateTime.Now;

            Assert.Equal("Sometime", dt.HasValue ? "Sometime" : "Never");
        }

        [Fact]
        public void ReturnRightValueIfExpressionIsFalse()
        {
            DateTime? dt = null;

            Assert.Equal("Never", dt.HasValue ? "Sometime" : "Never");
        }
    }
}
