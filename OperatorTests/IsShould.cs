using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace OperatorTests
{
    public class IsShould
    {
        #region Helpers

        private interface ISomeInterface
        {
            bool SomeMethod();
        }

        private class SomeBaseClass : ISomeInterface
        {
            bool ISomeInterface.SomeMethod()
            {
                return true;
            }
        }

        private class SomeDerivedClass : SomeBaseClass { }

        #endregion

        [Fact]
        public void ReturnTrueIfTypeIsCompatibleWithInstance()
        {
            var sdc = new SomeDerivedClass();
            Assert.True(sdc is SomeBaseClass);
        }

        [Fact]
        public void ReturnFalseIfTypeIsIncompatibleWithInstance()
        {
            var sbc = new SomeBaseClass();
            Assert.False(sbc is SomeDerivedClass);
        }

        #region Pattern Matching

        [Fact]
        public void WorkWithTheTypePattern()
        {
            var sdc = new SomeDerivedClass();

            if (sdc is SomeBaseClass sbc)
            {
                // I can use sbc in here
                Assert.NotNull(sbc);
            }

            // sbc is declared here, but compiler says it may not be initialized
            // Assert.NotNull(sbc);

            if (sdc is ISomeInterface)
            {
                var i = (ISomeInterface)sdc;
                i.SomeMethod();
            }
            
            if (sdc is ISomeInterface someInterface)
            {
                someInterface.SomeMethod();
            }

            // instead of
            if (sdc is SomeBaseClass)
            {
                var x = (SomeBaseClass)sdc;
                // I can use x in here
            }

            // x is out of scope here

            if (sdc is ISomeInterface si)
            {
                // I can use si in here
                Assert.NotNull(si);
            }

            // si is declared here, but compiler says it may not be initialized
            // Assert.NotNull(si);
        }

        [Fact]
        public void WorkWithTheConstantPattern()
        {
            #region null
            // Constant pattern works with null
            bool isNull = false;

            object o = null;

            if (o is null)
                isNull = true;

            Assert.True(isNull);
            #endregion

            #region constant value

            const string constantString = "Constant String";

            bool firstStringMatch  = false;
            bool secondStringMatch = false;

            if (new StringBuilder("some string").ToString() is constantString)
                firstStringMatch = true;

            if (new StringBuilder("Constant ").Append("String").ToString() is constantString)
                secondStringMatch = true;

            Assert.False(firstStringMatch);
            Assert.True(secondStringMatch);

            #endregion

        }

        [Fact]
        public void WorkWithVarPattern()
        {
            var parsed = -1;
            var s = new List<string> {"5", "3"};

            // used to declare temporary variable v
            if (s.FirstOrDefault(o => o != null) is var v
                && int.TryParse(v, out var n))
            {
                parsed = n;
            }

            // v is available here

            Assert.Equal(5, parsed);
        }

        #endregion
    }
}
