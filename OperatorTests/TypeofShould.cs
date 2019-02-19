using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace OperatorTests
{
    public class TypeofShould
    {
        #region Helpers

        private class SomeClass {}

        #endregion

        [Fact]
        public void ReturnSameTypeAsInstanceGetTypeMethod()
        {
            Assert.Equal(typeof(int), 5.GetType());

            Assert.Equal(typeof(SomeClass), new SomeClass().GetType());
        }
    }
}
