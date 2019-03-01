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
            var boolean = true;

            Assert.Equal("Sometime", boolean ? "Sometime" : "Never");
        }

        [Fact]
        public void ReturnRightValueIfExpressionIsFalse()
        {
            var boolean = false;

            Assert.Equal("Never", boolean ? "Sometime" : "Never");
        }
    }
}
