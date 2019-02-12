using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace MethodTests
{
    public class ReturnValuesShould
    {

        #region Helpers

        private static class HelperMethods
        {
            private static int _someIntegerbacker = 0;

            public static ref int GetIntegerValueBackByReference()
            {
                // ref local - can only use things that may be returned by reference
                // ref int refInteger = 10;
                ref int refInteger = ref _someIntegerbacker;
                return ref refInteger;
            }

            // Expression body method example
            public static ref int GetIntegerValueBackByValue() => ref _someIntegerbacker;
            // instead of this...
            //{
            //    return ref _someIntegerbacker;
            //}

        public static int SomeInteger
            {
                get { return _someIntegerbacker; }
                set { _someIntegerbacker = value; }
            }
        }

        private class MyClass
        {
            public int X { get; set; }
        }

        #endregion

        [Fact]
        public void NotBeModifiableWhenReturnedByValue()
        {
            HelperMethods.SomeInteger = 10;

            var returned = HelperMethods.GetIntegerValueBackByValue();

            returned = 100;

            Assert.Equal(100, returned);
            // the value has not changed
            Assert.Equal(10, HelperMethods.SomeInteger);
        }

        [Fact]
        public void BeModifiableWhenReturnedByReference()
        {
            HelperMethods.SomeInteger = 10;

            ref int returned = ref HelperMethods.GetIntegerValueBackByReference();

            returned = 100;

            Assert.Equal(100, returned);
            // the referenced value has changed
            Assert.Equal(100, HelperMethods.SomeInteger);
        }
    }
}
