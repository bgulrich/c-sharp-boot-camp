using System;
using Xunit;


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
        public void ReturnIfSomeUIntIsInRange()
        {
            var someUint = new UInt32[]
                           {
                               0,  1000000000,
                           };

            Assert.InRange(someUint[0], UInt32.MinValue, UInt32.MaxValue);
            Assert.InRange(someUint[0], UInt32.MinValue, UInt32.MaxValue);


        }

        [Fact]
        public void ReturnIfUIntIsOutOfRange()
        {
            var someUint = new UInt32[]
                           {
                              500000000,
                           };
            ;
            Assert.NotInRange( someUint[0] * (-1), UInt32.MinValue, UInt32.MaxValue);


        }

        [Fact]
        public void ThrowOverflowExceptionIfUIntIsOutOfRange()
        {
            var someUint = new UInt32[]
                           {
                               900000000,
                           };
            ;
            // well there is definitely overflow but not sure how to catch it.
            Assert.ThrowsAny<OverflowException>(() => someUint[0] * (100000));


        }

    }
}
