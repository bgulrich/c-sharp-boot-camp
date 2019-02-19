using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace BuiltInTypesTests
{
    public class BoolsShould
    {
        [Theory]
        [InlineData("true", true, true)]
        [InlineData("false", true, false)]
        [InlineData("True", true, true)]
        [InlineData("False", true, false)]
        [InlineData("TrUe", true, true)]
        [InlineData("FaLSe", true, false)]
        [InlineData("t", false, null)]
        [InlineData("f", false, null)]
        [InlineData("T", false, null)]
        [InlineData("F", false, null)]
        public void ParseValuesCorrectly(string input, bool parses, bool? value)
        {
            Assert.Equal(parses, bool.TryParse(input, out bool parsedResult));

            if (parses)
                Assert.Equal(value.Value, parsedResult);
        }

        [Fact]
        public void DefineStringValues()
        {
            Assert.Equal("True", bool.TrueString);
            Assert.Equal("False", bool.FalseString);
        }

        [Fact]
        public void BeAutoRecognizedWhenAssignedTrueOrFalseValue()
        {
            var unknownType = true;

            Assert.Equal("System.Boolean", unknownType.GetType().ToString());
        }

        [Fact]
        public void BeAssignedThroughDifferentApproach()
        {
            bool myBool_1 = 5 > 6;
            bool myBool_2 = 3 + 4 == 8;
            bool myBool_3 = false;

            Assert.False(myBool_1);
            Assert.False(myBool_2);
            Assert.False(myBool_3);
        }

        [Fact]
        public void BeCastedToLargerTypes()
        {
            var myBool = true;
            byte myByte = Convert.ToByte(myBool);
            int myInt = Convert.ToInt32(myBool);
            double myDouble = Convert.ToDouble(myBool);

            Assert.Equal(1, myByte);
            Assert.Equal(1, myInt);
            Assert.Equal(1, myDouble);
        }

        [Fact]
        public void HasTheSmallestSize()
        {
            Assert.True(sizeof(Boolean) < sizeof(Char));
            Assert.True(sizeof(Char) < sizeof(Int32));
            Assert.True(sizeof(Int32) < sizeof(Double));
            Assert.True(sizeof(Double) < sizeof(Decimal));
            Assert.Equal(1, sizeof(Boolean));
        }
    }
}
