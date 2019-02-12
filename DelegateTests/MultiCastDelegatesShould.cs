using Moq;
using System;
using Xunit;

namespace DelegateTests
{
    public class MulticastDelegatesShould
    {
        private Mock<ISomeInterface> _mockInterface1 = new Mock<ISomeInterface>();
        private Mock<ISomeInterface> _mockInterface2 = new Mock<ISomeInterface>();

        private delegate int CountLettersDelegate(string input);

        [Fact]
        public void PassVariablesToAllSubscribedMethods()
        {
            var input = "My input string";

            _mockInterface1.Setup(i => i.CountLetters(It.Is<string>(s => s == input)))
                           .Verifiable();

            _mockInterface2.Setup(i => i.CountLetters(It.Is<string>(s => s == input)))
                           .Verifiable();

            CountLettersDelegate del1 = _mockInterface1.Object.CountLetters;
            CountLettersDelegate del2 = _mockInterface2.Object.CountLetters;

            CountLettersDelegate mcd = del1 + del2;

            var output = mcd(input);

            _mockInterface1.Verify();
            _mockInterface2.Verify();
        }

        [Fact]
        public void ReturnValueFromLastInvokedMethod()
        {
            var input = "My input string";

            _mockInterface1.Setup(i => i.CountLetters(It.Is<string>(s => s == input)))
                           .Returns(1);

            _mockInterface2.Setup(i => i.CountLetters(It.Is<string>(s => s == input)))
                           .Returns(2);

            CountLettersDelegate del1 = _mockInterface1.Object.CountLetters;
            CountLettersDelegate del2 = _mockInterface2.Object.CountLetters;

            CountLettersDelegate mcd = del1 + del2;

            var output = mcd(input);

            Assert.Equal(2, output);
        }

        [Fact]
        public void NotInvokeRemovedMethods()
        {
            var input = "My input string";

            _mockInterface1.Setup(i => i.CountLetters(It.Is<string>(s => s == input)))
                           .Verifiable();

            _mockInterface2.Setup(i => i.CountLetters(It.Is<string>(s => s == input)))
                           .Verifiable();

            CountLettersDelegate del = _mockInterface1.Object.CountLetters;
            del += _mockInterface2.Object.CountLetters;

            del -= _mockInterface1.Object.CountLetters;

            var output = del(input);

            _mockInterface1.Verify(i => i.CountLetters(It.IsAny<string>()), Times.Never);
            _mockInterface2.Verify();
        }

    }
}
