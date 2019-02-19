using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace BuiltInTypesTests
{
    public class CharsShould
    {
        [Fact]
        public void AcceptUnicode()
        {
            // Currency Symbol
            char yen = '\u00A5';
            Assert.Equal('¥', yen);

            // Dialectic Symbol
            char cedilla = '\u00E7';
            Assert.Equal('ç', cedilla);

            // Fraction
            char oneFourth = '\u00BC';
            Assert.Equal('¼', oneFourth);
        }

        [Fact]
        public void AcceptLiteralCharacters()
        {
            char charInput;

            try
            {
                charInput = 'A';
                charInput = 'b';
                charInput = '\\';
            }
            catch
            {
                Assert.True(false); // fail if exception thrown
            }

            // Illegal
            //string stringInput = 'A';
            //stringInput = 'b';
            //stringInput = '\\';
        }

        [Theory]
        [InlineData("C Sharp Bootcamp Rules","43 20 53 68 61 72 70 20 42 6F 6F 74 63 61 6D 70 20 52 75 6C 65 73")]
        [InlineData("3.14159","33 2E 31 34 31 35 39")]
        public void ConvertBetweenHexadecimalAndAlphanumerical(string input, string hexOutput)
        {
            char[] values = input.ToCharArray();
            var hexadecimalString = new StringBuilder();
            var stringValue = new StringBuilder();

            // Convert string to hexadecimal
            foreach (char letter in values)
            {
                int value = Convert.ToInt32(letter);
                hexadecimalString.Append(value.ToString("X2") + " ");
            }
            hexadecimalString.Remove(hexadecimalString.Length - 1, 1); // get rid of last space

            Assert.Equal(hexOutput, hexadecimalString.ToString());

            // Convert hexadecimal back to string
            string[] hexOutputSplit = hexOutput.Split(' ');

            foreach (string hex in hexOutputSplit)
            {
                int value = Convert.ToInt32(hex, 16);
                char charValue = (char)value;
                stringValue.Append(charValue.ToString());
            }

            Assert.Equal(input,stringValue.ToString());

        }
    }
}
