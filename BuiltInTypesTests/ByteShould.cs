using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace BuiltInTypesTests
{
    public class ByteShould
    {
        [Theory]
        [InlineData(20,20)]
        [InlineData(40,40)]
        [InlineData(202,202)]
        [InlineData(255,255)]
        public void BeSameAsUnsigned8BitInteger(byte b, int i)
        {
            Assert.Equal(b, i);
        }

        [Theory]
        [InlineData(255,1,0)]
        [InlineData(5,255,4)]
        [InlineData(byte.MaxValue,byte.MaxValue,254)]
        public void RecountFromZeroWhenMoreThan255(byte b, byte b1, byte expected)
        {
            Assert.Equal(expected,(byte)(b+b1));
        }

        [Fact]
        public void ThrowOverflowExceptionIfMoreThan255()
        {
            int number = Int32.MaxValue;
            Assert.Throws<OverflowException>(()=>Convert.ToByte(number));
        }

    }
}
