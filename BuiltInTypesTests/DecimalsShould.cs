using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace BuiltInTypesTests
{
    public class DecimalsShould
    {
        [Fact]
        public void HaveSuffixOfM()
        {
            var proper = 3.14159m;
            // Illegal
            // decimal improper = 3.14159;

            Assert.Equal("3.14159", proper.ToString());
        }

        [Theory]
        [InlineData(12345678910111213141516171819.2, true)] // precision of 29
        [InlineData(1.23456789101112131415161718192, true)]
        public void HavePrecisionUptoTwentyNineSignficantDigits(decimal test, bool value)
        {
            decimal maxDecimal = Decimal.MaxValue;
            decimal minDecimal = Decimal.MinValue;

            Assert.InRange(test, minDecimal, maxDecimal);
        }

        [Fact]
        public void AllowCurrencyFormating()
        {
            decimal a = 19.99m;
            Assert.Equal("$19.99", a.ToString("C2"));

            // Rounds up when decimal place exceeds cent
            a = 0.9999m;
            Assert.Equal("$1.00",a.ToString("C2"));
            
            // Auto fills in zeros for whole numbers
            a = 9999999999999999999999999999m;
            Assert.Equal("$9,999,999,999,999,999,999,999,999,999.00",a.ToString("C2"));
        }
    }
}
