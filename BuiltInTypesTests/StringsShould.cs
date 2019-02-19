using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace BuiltInTypesTests
{
    public class StringsShould
    {

        [Fact]
        public void BeLessPerformantThanStringBuilderWhenModifying()
        {
            var strings = new[] {"Hello", "oh my!", "what?", "C#", "This is a long string for my array", "blah"};

            var bigString = string.Empty;
            var stringBuilder = new StringBuilder();
            var random = new Random();

            var stringStart = DateTime.Now;

            for (var i = 0; i < 40000; ++i)
            {
                bigString += strings[random.Next(strings.Length)];
            }

            var stringTime = DateTime.Now - stringStart;
            var stringBuilderStart = DateTime.Now;

            for (var i = 0; i < 40000; ++i)
            {
                stringBuilder.Append(strings[random.Next(strings.Length)]);
            }

            bigString = stringBuilder.ToString();

            var stringBuilderTime = DateTime.Now - stringBuilderStart;

            // string implementation should take at least 100 times as long
            var slowDown = stringTime.TotalSeconds / stringBuilderTime.TotalSeconds;
            Assert.InRange(slowDown, 100, double.MaxValue);
        }

        [Fact]
        public void BeAnArrayOfCharacters()
        {
            var string1 = "Walmart Rocks!";
            var char_array = new char[] { 'W', 'a', 'l', 'm', 'a', 'r', 't', ' ', 'R', 'o', 'c', 'k', 's', '!' };
            //Assert they are equal
            Assert.Equal(string1,char_array);
        }
        [Fact]
        public void BeAbletoBeDeclaredEitherWay()
        {
            var string1 = "Walmart Rocks!";
            var string2 = new string("Walmart Rocks!");

            Assert.Equal(string1, string2);
        }
        [Theory]
        [InlineData("Walmart")]
        [InlineData("Rocks")]
        [InlineData("Walmart Rocks!!")]
        public void BeInheritedFromObject(string inputString)
        {
            object stringObject = (object)inputString;

            Assert.Equal(stringObject, inputString);

        }
        
    }
}
