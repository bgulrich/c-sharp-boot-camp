using System;
using Xunit;


namespace BuiltInTypesTests
{
    public class UShortShould
    {

        [Fact]
        public void BeOfTheSameTypesAsTheirAliases()
        {
            Assert.Equal(typeof(ushort),  typeof(UInt16));

        }

        [Fact]
        public void ReturnIfSomeUShortIsInRange()
        {
            var someUshort = 50000;

            Assert.InRange(someUshort, UInt16.MinValue, UInt16.MaxValue);


        }

        [Fact]
        public void ReturnIfUShortIsOutOfRange()
        {
            var someUshort = 50000;

            ;
            Assert.NotInRange( someUshort * (-1), UInt16.MinValue, UInt16.MaxValue);


        }


    }
}
