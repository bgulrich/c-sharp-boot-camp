using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ThreadBasedTaskTests
{
    public class ContinueWithShould
    {
        [Fact]
        public async Task ReturnImmediately()
        {
            var counter = 0;

            var t = Task.Factory.StartNew(() =>
            {
                Task.Delay(100).Wait();
                return ++counter;
            });

            var continuationTask = t.ContinueWith((continueFromTask) =>
            {
                Task.Delay(100).Wait();
                return continueFromTask.Result + 1;
            });

            counter = 100;

            Assert.Equal(100, counter);
            Assert.Equal(101, await t);
            Assert.Equal(102, await continuationTask);
            Assert.Equal(101, counter);
        }
    }
}
