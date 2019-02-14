using System;
using Xunit;

namespace BuiltInTypesTests
{
    public class BuiltInTypesShould
    {
        [Fact]
        public void BeOfTheSameTypesAsTheirAliases()
        {
            Assert.Equal(typeof(bool),    typeof(Boolean));
            Assert.Equal(typeof(byte),    typeof(Byte));
            Assert.Equal(typeof(sbyte),   typeof(SByte));
            Assert.Equal(typeof(char),    typeof(Char));
            Assert.Equal(typeof(decimal), typeof(Decimal));
            Assert.Equal(typeof(double),  typeof(Double));
            Assert.Equal(typeof(float),   typeof(Single));
            Assert.Equal(typeof(int),     typeof(Int32));
            Assert.Equal(typeof(uint),    typeof(UInt32));
            Assert.Equal(typeof(long),    typeof(Int64));
            Assert.Equal(typeof(ulong),   typeof(UInt64));
            Assert.Equal(typeof(object),  typeof(Object));
            Assert.Equal(typeof(short),   typeof(Int16));
            Assert.Equal(typeof(ushort),  typeof(UInt16));
            Assert.Equal(typeof(string),  typeof(String));
        }
    }
}
