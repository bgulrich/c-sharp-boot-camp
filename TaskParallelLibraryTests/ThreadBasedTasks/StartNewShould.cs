using System;
using System.Threading.Tasks;
using Xunit;

namespace ThreadBasedTaskTests
{
    public class TaskFactoryStartNewShould
    {
        [Fact]
        public async Task ReturnImmediately()
        {
            int counter = 0;

            var t = Task.Factory.StartNew(() =>
            {
                Task.Delay(100).Wait();
                return counter + 1;
            });

            counter = 100;

            var taskCounter = await t;

            Assert.Equal(101, taskCounter);
        }
    }
}
