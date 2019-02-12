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

        public DelegatesShould()
        {

        }

        [Fact]
        public void PassVariableToMethod()
        {
            _mockInterface.Invocations.Clear();

            var input = "My input string";

            _mockInterface.Setup(i => i.CountLetters(It.Is<string>(s => s == input)))
                          .Verifiable();


            //CountLettersDelegate d = _mockInterface.Object.CountLetters;
            // OR 
            var del = new CountLettersDelegate(_mockInterface.Object.CountLetters);

            del(input);

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

            Assert.Equal(input.Length, result);
        }
    }
}
