using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace TPLTests
{
    public class ConfigureAwaitShould
    {
        [Fact]
        public async Task ContinueOnSameThreadIfContinueOnCapturedContextIsTrue()
        {
            await Task.Delay(100);

            var startingSynchronizationContext = SynchronizationContext.Current;

            await Task.Delay(100).ConfigureAwait(true);

            var endingSynchronizationContext = SynchronizationContext.Current;

            Assert.Equal(startingSynchronizationContext, endingSynchronizationContext);
        }

        [Fact]
        public async Task PotentiallyContinueOnDifferentContextIfContinueOnCapturedContextIsFalse()
        {
            await Task.Delay(100);

            var previousSynchronizationContext = SynchronizationContext.Current;
            var changedContexts = 0;

            for (var i = 0; i < 10; ++i)
            {
                await Task.Delay(100).ConfigureAwait(false);

                var currentSynchronizationContext = SynchronizationContext.Current;

                if (previousSynchronizationContext != currentSynchronizationContext)
                    ++changedContexts;

                previousSynchronizationContext = currentSynchronizationContext;
            }

            Assert.NotEqual(0, changedContexts);
        }
    }
}
