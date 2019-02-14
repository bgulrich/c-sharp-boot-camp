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
            var inputString = "My input string";
            var inputInt = 100;

            _mockInterface.Setup(m => m.SomeAction(It.Is<string>(s => s == inputString),
                                                   It.Is<int>(i => i == inputInt)))
                          .Verifiable();

            // Action<string, int> del = _mockInterface.Object.SomeAction;
            // OR
            var del = new Action<string, int>(_mockInterface.Object.SomeAction);

            del(inputString, inputInt);

            // make sure method called with correct arguments
            _mockInterface.Verify();
        }
    }
}
