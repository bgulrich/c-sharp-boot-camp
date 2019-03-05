using Moq;
using System;
using Xunit;

namespace DelegateTests
{
    public class DelegatesShould
    {
        private Mock<ISomeInterface> _mockInterface = new Mock<ISomeInterface>();

        // declare new delegate type
        private delegate int CountLettersDelegate(string input);

        private static class SomeStaticClass
        {
            public static int CountLetters(string input)
            {
                return input.Length;
            }
        }

        [Fact]
        public void PassVariableToMethod()
        {
            var input = "My input string";

            _mockInterface.Setup(i => i.CountLetters(It.Is<string>(s => s == input)))
                          .Verifiable();

            //CountLettersDelegate d = _mockInterface.Object.CountLetters;
            // OR 
            var del = new CountLettersDelegate(_mockInterface.Object.CountLetters);

            del(input);

            // make sure the delegate called the method on the mock
            _mockInterface.Verify();
        }

        [Fact]
        public void ReturnValueFromMethod()
        {
            var input = "My input string";

            _mockInterface.Setup(i => i.CountLetters(It.Is<string>(s => s == input)))
                          .Returns(input.Length);

            var del = new CountLettersDelegate(_mockInterface.Object.CountLetters);

            var result = del(input);

            // make sure the delegate returns the same value as the method
            Assert.Equal(input.Length, result);
        }

        [Fact]
        public void WorkWithStaticMethodsToo()
        {
            var del = new CountLettersDelegate(SomeStaticClass.CountLetters);

            var input = "Howdy!!!";

            Assert.Equal(input.Length, del(input));
        }

        [Fact]
        public void WorkWithAnonymousMethodsToo()
        {
            CountLettersDelegate del = (string s) => { return s.Length * s.Length; };

            var input = "Howdy!!!";

            Assert.Equal(input.Length * input.Length, del(input));
        }
    }
}
