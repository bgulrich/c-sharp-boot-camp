using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace BuiltInTypesTests
{
    public class LongShould
    {
        [Fact]
        public void ThrowExceptionIfExceedsTheRange()
        {
            long l = long.MaxValue;
            string lStr = l + "1"; //This will make the string out of the range of long
            Assert.Throws<OverflowException>(() => long.Parse(lStr));
        }
        [Fact]
        public void BeASigned64BitInteger()
        {
            Assert.Equal(typeof(long), typeof(Int64));
        }
        [Fact]
        public void ImplicitlyConvertFromHex()
        {
            var hex = 0x1000000;
            long l = hex;
            Assert.Equal(hex, l);
        }
    }
}
