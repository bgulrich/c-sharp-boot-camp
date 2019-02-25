using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace OperatorTests
{
    public class AsShould
    {
        #region Helpers

        private class SomeBaseClass { }

        private class SomeDerivedClass : SomeBaseClass { }

        #endregion

        [Fact]
        public void SucceedIfTypeIsCompatibleWithInstance()
        {
            var sdc = new SomeDerivedClass();
            var sbc = sdc as SomeBaseClass;

            Assert.NotNull(sbc);

            // have to check for null before using
        }

        [Fact]
        public void ReturnNullIfTypeIsIncompatibleWithInstance()
        {
            var sbc = new SomeBaseClass();
            var sdc = sbc as SomeDerivedClass;

            Assert.Null(sdc);
        }
    }
}
