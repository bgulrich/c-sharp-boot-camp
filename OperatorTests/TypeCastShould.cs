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

        private interface SomeInterface {}

        private class SomeDerivedClass : SomeBaseClass, SomeInterface { }

        #endregion

        [Fact]
        public void SucceedIfTypeIsCompatibleWithInstance()
        {
            var sdc = new SomeDerivedClass();

            // cast to parent class
            var sbc = (SomeBaseClass)sdc;
            Assert.NotNull(sbc);

            // cast to implemented interface
            var i = (SomeInterface)sdc;
            Assert.NotNull(i);
        }

        [Fact]
        public void ThrowAnInvalidCastExceptionIfTypeIsIncompatibleWithInstance()
        {
            var sbc = new SomeBaseClass();
            Assert.Throws<InvalidCastException>(() => { var sdc = (SomeDerivedClass)sbc; });

            Assert.Throws<InvalidCastException>(() => { var i = (SomeInterface)sbc; });

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
