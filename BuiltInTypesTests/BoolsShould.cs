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
    }
}
