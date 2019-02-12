using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace DelegateTests
{
    public class FunctionsShould
    {
        private Mock<ISomeInterface> _mockInterface = new Mock<ISomeInterface>();

        [Fact]
        public void PassVariableToMethod()
        {
            _mockInterface.Invocations.Clear();

            var input = "My input string";

            _mockInterface.Setup(i => i.CountLetters(It.Is<string>(s => s == input)))
                          .Verifiable();

            // Func<string, int> del = _mockInterface.Object.CountLetters;
            // OR
            var del = new Func<string, int>(_mockInterface.Object.CountLetters);

            del(input);

            _mockInterface.Verify();
        }

        [Fact]
        public void ReturnValueFromMethod()
        {
            var input = "My input string";

            _mockInterface.Setup(i => i.CountLetters(It.Is<string>(s => s == input)))
                          .Returns(input.Length);

            var del = new Func<string, int>(_mockInterface.Object.CountLetters);

            var result = del(input);

            Assert.Equal(input.Length, result);
        }
    }
}
