using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace OperatorTests
{
    public class TypeCastShould
    {
        #region Helpers

        private class SomeBaseClass { }

        private class SomeDerivedClass : SomeBaseClass { }

        #endregion

        [Fact]
        public void SucceedIfTypeIsCompatibleWithInstance()
        {
            var sdc = new SomeDerivedClass();
            var sbc = (SomeBaseClass)sdc;
            Assert.NotNull(sbc);
        }

        [Fact]
        public void ThrowAnInvalidCastExceptionIfTypeIsIncompatibleWithInstance()
        {
            var sbc = new SomeBaseClass();
            Assert.Throws<InvalidCastException>(() => { var sdc = (SomeDerivedClass)sbc; });
        }

        [Fact]
        public void AlwaysSucceedIfInstanceIsNull()
        {
            SomeBaseClass sbc = null;

            // incompatible but still works on null
            var sdc = (SomeDerivedClass)sbc;

            Assert.Null(sdc);

            sdc = (SomeDerivedClass)null;

            Assert.Null(sdc);
        }
    }
}
