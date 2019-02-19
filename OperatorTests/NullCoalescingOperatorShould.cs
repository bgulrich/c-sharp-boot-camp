using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace OperatorTests
{
    public class NullCoalescingOperatorShould
    {
        #region Helpers

        private class SomeClass
        {
            public int GetAFive() { return 5; }
        }

        #endregion

        [Fact]
        public void ReturnRightValueIfValueIsNull()
        {
            SomeClass sc = null;
            Assert.Equal(10, sc?.GetAFive() ?? 10);
        }

        [Fact]
        public void ReturnLeftValueIfValueIsNotNull()
        {
            SomeClass sc = new SomeClass();
            Assert.Equal(5, sc?.GetAFive() ?? 10);
        }
    }
}
