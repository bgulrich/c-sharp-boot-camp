using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace TPLTests.AsyncMethods
{
    public class AsyncMethodsShould
    {
        private class MyClass
        {
            public int SomeValue { get; set; }
        }

        private async Task IncrementValueAsync(MyClass instance)
        {
            // simulate some long-running work
            await Task.Delay(1000);

            ++instance.SomeValue;
        }


        [Fact]
        public void ExecuteImmediatelyIfNotAwaited()
        {
            var instance = new MyClass {SomeValue = 10};

            // don't await
            IncrementValueAsync(instance);

            Assert.Equal(10, instance.SomeValue);
        }

        [Fact]
        public async Task SuspendExecutionIfAwaited()
        {
            var instance = new MyClass { SomeValue = 10 };

            // await
            await IncrementValueAsync(instance);

            Assert.Equal(11, instance.SomeValue);
        }

    }
}
