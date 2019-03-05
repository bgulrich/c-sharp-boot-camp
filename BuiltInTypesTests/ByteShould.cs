using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace BuiltInTypesTests
{
    public class ByteShould
    {
        [Theory]
        [InlineData(255,1,0)]
        [InlineData(5,255,4)]
        [InlineData(byte.MaxValue,byte.MaxValue,254)]
        public void RecountFromZeroWhenMoreThan255(byte b, byte b1, byte expected)
        {
            Assert.Equal(expected,(byte)(b+b1));
        }

        [Fact]
        public void ThrowOverflowExceptionCheckedOperationOverflows()
        {
            byte b = byte.MaxValue;

            Assert.Throws<OverflowException>(()=> checked(b = b++));
        }

    }
}
