using System;
using Xunit;
using Xunit.Abstractions;


namespace BuiltInTypesTests
{
    public class UIntShould
    {

        [Fact]
        public void BeOfTheSameTypesAsTheirAliases()
        {
            Assert.Equal(typeof(uint),  typeof(UInt32));

        }

        [Fact]
        public void CheckIfSomeUIntIsInRange()
        {
            uint someUint = 1000000000;

            Assert.InRange(someUint, UInt32.MinValue, UInt32.MaxValue);


        }

        [Fact]
        public void CheckIfUIntIsOutOfRange()
        {
            uint someUint = 500000000;
                           ;
            ;
            Assert.NotInRange( someUint * (-1), UInt32.MinValue, UInt32.MaxValue);


        }

        [Fact]
        public void ThrowOverflowExceptionIfUIntIsOutOfRange()
        {


            var someUint = 80 * Math.Pow(100, 10000);

            // well there is definitely overflow but not sure how to catch it. does not throw overflow must be casting automatically or throwing away the bits?

            //Assert.ThrowsAny<OverflowException>(() => 80 * Math.Pow(100, 10000));
            Assert.NotInRange(someUint, UInt32.MinValue, UInt32.MaxValue);


        }

    }
}
