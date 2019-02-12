using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace DelegateTests
{
    public class ActionsShould
    {
        private Mock<ISomeInterface> _mockInterface = new Mock<ISomeInterface>();

        [Fact]
        public void PassVariableToMethod()
        {
            _mockInterface.Invocations.Clear();

            var inputString = "My input string";
            var inputInt = 100;

            _mockInterface.Setup(m => m.SomeAction(It.Is<string>(s => s == inputString),
                                                   It.Is<int>(i => i == inputInt)))
                          .Verifiable();

            // Func<string, int> del = _mockInterface.Object.CountLetters;
            // OR
            var del = new Action<string, int>(_mockInterface.Object.SomeAction);

            del(inputString, inputInt);

            _mockInterface.Verify();
        }
    }
}
