using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace BuiltInTypesTests
{
    public class IntsShould
    {
        [Fact]
        public void OverflowWhenOutOfRange()
        {
            int maxInteger = int.MaxValue;
            int minInteger = int.MinValue;

            Assert.Equal(minInteger, (maxInteger + 1));
            Assert.Equal(-2147483648, (maxInteger + 1));
            Assert.Equal(maxInteger, (minInteger - 1));
            Assert.Equal(2147483647, (minInteger - 1));
        }

        [Fact]
        public void DifferValuesBetweenDifferentInt()
        {
            int maxInt_16 = Int16.MaxValue;
            int minInt_16 = Int16.MinValue;
            int maxInt_32 = Int32.MaxValue;
            int minInt_32 = Int32.MinValue;
            long maxInt_64 = Int64.MaxValue;
            long minInt_64 = Int64.MinValue;
 
            Assert.Equal(32767, maxInt_16);
            Assert.Equal(-32768, minInt_16);
            Assert.Equal(short.MaxValue, maxInt_16);
            Assert.Equal(short.MinValue, minInt_16);
            Assert.Equal(2147483647, maxInt_32);
            Assert.Equal(-2147483648, minInt_32);
            Assert.Equal(int.MaxValue, maxInt_32);
            Assert.Equal(int.MinValue, minInt_32);
            Assert.Equal(9223372036854775807, maxInt_64);
            Assert.Equal(-9223372036854775808, minInt_64);
            Assert.Equal(long.MaxValue, maxInt_64);
            Assert.Equal(long.MinValue, minInt_64);
        }

        [Fact]
        public void NotRoundWhenCasting()
        {
            double myDouble_1 = 0.358496782;
            double myDouble_2 = 6.68;

            Assert.Equal(Math.Floor(myDouble_1), (int)myDouble_1);
            Assert.Equal(Math.Round(myDouble_1), (int)myDouble_1);

            Assert.Equal(Math.Floor(myDouble_2), (int)myDouble_2);
            Assert.NotEqual(Math.Round(myDouble_2), (int)myDouble_2);
        }


    }
}
