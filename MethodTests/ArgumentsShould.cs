using System;
using Xunit;

namespace MethodTests
{
    public class ArgumentsShould
    {
        #region Helpers

        private static class HelperMethods
        {
            // modifying input parameter with 'in' would be illegal since the value is modified
            public static void ModifyAnIntegerPassedByValue(int input)
            {
                input = input + 5;
            }

            public static void ModifyAnIntegerPassedByReference(ref int input)
            {
                input = input + 5;
            }

            public static void CantModifyIntegerPassedAsIn(in int input)
            {
                // illegal
                //input = input + 1;
            }

            public static void ModifyAPassedReferenceType(MyClass c)
            {
                c.X = c.X + 5;
            }

            private static int SomeIntegerBacker = 0;

            public static ref int GetIntegerValueBackByReference()
            {
                // ref local - can only use things that may be returned by reference
                // ref int refInteger = 10;
                ref int refInteger = ref SomeIntegerBacker;
                return ref refInteger;
            }

            public static int SomeInteger => SomeIntegerBacker;
        }

        private class MyClass
        {
            public int X { get; set; }
        }

        #endregion

        [Fact]
        public void NotBeModifiableWhenPassedByValue()
        {
            var integer = 5;

            Assert.Equal(5, integer);

            HelperMethods.ModifyAnIntegerPassedByValue(integer);

            // no effect
            Assert.Equal(5, integer);
        }

        [Fact]
        public void BeModifiableWhenIsValueButPassedByReference()
        {
            var integer = 5;

            Assert.Equal(5, integer);

            HelperMethods.ModifyAnIntegerPassedByReference(ref integer);

            // modified by method
            Assert.Equal(10, integer);
        }

        [Fact]
        public void BeModifiableWhenIsAReferenceType()
        {
            var mc = new MyClass { X = 5 };

            Assert.Equal(5, mc.X);

            HelperMethods.ModifyAPassedReferenceType(mc);

            // modified by method
            Assert.Equal(10, mc.X);
        }
    }
}
