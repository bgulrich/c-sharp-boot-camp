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
            var counter = 0;

            var t = Task.Factory.StartNew(() =>
            {
                Task.Delay(100).Wait();
                return counter + 1;
            });

            counter = 100;

            var taskCounter = await t;

            Assert.Equal(101, taskCounter);
        }

        [Fact]
        public async Task ExecuteSynchronousCode()
        {
            // Task<int>
            var t = Task.Factory.StartNew(() =>
            {
                var counter = 0;

                for (var i = 0; i < 100; ++i)
                    counter = i;

                return counter;
            });

            var result = await t;

            Assert.Equal(99, result);
        }

        [Fact]
        public async Task ExecuteAsynchronousCode()
        {
            // Task<Task<int>>
            var t = Task.Factory.StartNew(async () =>
            {
                var counter = 0;

                for (var i = 0; i < 100; ++i)
                {
                    // simulate asynchronous work
                    await Task.Delay(100);
                    counter = i;
                }

                return counter;
            });

            var result = await t.Unwrap();

            Assert.Equal(99, result);
        }
    }
}
