using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace BuiltInTypesTests
{
    public class DoublesShould
    {
        [Fact]
        public void BeRecognizedAsDoubleType()
        {
            var unknownTypeValue = 4.0;

            Assert.Equal("System.Double", unknownTypeValue.GetType().ToString());
        }


        [Fact]
        public void OverflowWhenOutOfRange()
        {
            double maxDouble = double.MaxValue;

            Assert.True(double.IsPositiveInfinity(maxDouble * 2));
        }

        [Fact]
        public void GivePreciseResultWhenConvertingToDecimal()
        {
            double addtionOfTwoDecimal_1 = 0.05 + 0.01;
            Assert.NotEqual(0.06, addtionOfTwoDecimal_1);
            Assert.Equal(0.060000000000000005, addtionOfTwoDecimal_1);

            decimal addtionOfTwoDecimal_2 =(decimal) (0.05 + 0.01);
            Assert.Equal((decimal)0.06, addtionOfTwoDecimal_2);

            double addtionOfTwoDecimal_3 = 123.3 / 100;
            Assert.NotEqual(1.233, addtionOfTwoDecimal_3);
            Assert.Equal(1.2329999999999999, addtionOfTwoDecimal_3);

            decimal addtionOfTwoDecimal_4 = (decimal)(123.3 / 100);
            Assert.Equal((decimal)1.233, addtionOfTwoDecimal_4);
        }
    }
}
