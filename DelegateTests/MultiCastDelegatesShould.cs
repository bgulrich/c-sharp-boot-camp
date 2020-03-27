using Moq;
using System;
using Xunit;

namespace DelegateTests
{
    public class MulticastDelegatesShould
    {
        private readonly Mock<ISomeInterface> _mockInterface1 = new Mock<ISomeInterface>();
        private readonly Mock<ISomeInterface> _mockInterface2 = new Mock<ISomeInterface>();

        private delegate int CountLettersDelegate(string input);

        [Fact]
        public void PassParametersToAllAddedMethods()
        {
            var input = "My input string";

            _mockInterface1.Setup(i => i.CountLetters(input))
                           .Verifiable();

            _mockInterface2.Setup(i => i.CountLetters(input))
                           .Verifiable();

            // Create two delegate instances and use +operator to combine them
            //CountLettersDelegate del1 = _mockInterface1.Object.CountLetters;
            //CountLettersDelegate del2 = _mockInterface2.Object.CountLetters;
            // create a multicast delegate that calls both methods
            //var mcd = del1 + del2;

            // OR create a delegate and use +=operator to add other method
            var mcd = new CountLettersDelegate(_mockInterface1.Object.CountLetters);
            mcd += _mockInterface2.Object.CountLetters;

            var output = mcd(input);

            // make sure both mock methods were called
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

            // create a multicast delegate that calls both methods
            var mcd = del1 + del2;

            var output = mcd(input);

            // make sure return value from last method is returned
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

            // remove the method from mock interface 1
            del -= _mockInterface1.Object.CountLetters;

            var output = del(input);

            // make sure mock interface 1 method wasn't called and 2 was called
            _mockInterface1.Verify(i => i.CountLetters(It.IsAny<string>()), Times.Never);
            _mockInterface2.Verify();
        }

    }
}
