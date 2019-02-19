using System;
using Xunit;
using Xunit.Abstractions;


namespace BuiltInTypesTests
{
    public class UIntShould
    {
        [Fact]
        public void BeOfTheSameTypesAsUInt32()
        {
            Assert.Equal(typeof(uint), typeof(UInt32));
        }

        [Fact]
        public void ThrowOverflowExceptionIfCheckedOperationCausesUIntIsOutOfRange()
        {
            var someUint = uint.MaxValue;

            Assert.Throws<OverflowException>(() => checked(someUint += 1));

            // value doesn't move
            Assert.Equal(uint.MaxValue, someUint);
        }

        [Fact]
        public void ParseValidUintValue()
        {
            var input = "568";
            Assert.Equal((uint)568, uint.Parse(input));
        }

        [Fact]
        public void ThrowFormatExceptionWhenParsingInvalidString()
        {
            var input = "blah";
            Assert.Throws<FormatException>(() => uint.Parse(input));
        }

        [Fact]
        public void FailToTryParseInvalidString()
        {
            var input = "fifty five";

            Assert.False(uint.TryParse(input, out uint parsed));

            Assert.Equal(default(uint), parsed);
        }
    }
}
