using System;
using Xunit;

namespace BuiltInTypesTests
{
    public class UShortShould
    {

        [Fact]
        public void BeOfTheSameTypesAsUInt16()
        {
            Assert.Equal(typeof(ushort),  typeof(UInt16));
        }

        [Fact]
        public void ThrowOverflowExceptionIfCheckedOperationCausesUIntIsOutOfRange()
        {
            var someUshort = ushort.MaxValue;

            Assert.Throws<OverflowException>(() => checked(someUshort += 1));

            // value doesn't move
            Assert.Equal(ushort.MaxValue, someUshort);
        }

        [Fact]
        public void ParseValidString()
        {
            var input = "568";
            Assert.Equal((ushort)568, ushort.Parse(input));
        }

        [Fact]
        public void ThrowFormatExceptionWhenParsingInvalidString()
        {
            var input = "blah";
            Assert.Throws<FormatException>(() => ushort.Parse(input));
        }

        [Fact]
        public void FailToTryParseInvalidString()
        {
            var input = "fifty five";

            Assert.False(ushort.TryParse(input, out ushort parsed));

            Assert.Equal(default(ushort), parsed);
        }
    }
}
